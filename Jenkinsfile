
pipeline {
    agent any

    environment {
        AWS_REGION = 'us-west-2'                        // AWS region
        AWS_CREDENTIALS = 'cred'                       // AWS credentials ID in Jenkins
        GIT_CREDENTIALS = 'git'                        // GitHub credentials ID
        DOCKER_CREDENTIALS = 'docker'                  // DockerHub credentials ID
        DOCKER_REPOSITORY = 'chandanuikey97/k8s-eks-image' // DockerHub repository base name
        EKS_CLUSTER_NAME = 'three-tier-cluster-2'      // EKS cluster name
        KUBECONFIG = '/var/lib/jenkins/.kube/config'   // Kubeconfig path
    }

    stages {
        stage('Checkout Code') {
            steps {
                checkout scm: [$class: 'GitSCM',
                               branches: [[name: '*/terraform-test']],
                               userRemoteConfigs: [[credentialsId: GIT_CREDENTIALS, url: 'https://github.com/chan-764/pyhtondeployment.git']]]
            }
        }

        stage('Terraform: Setup Infrastructure') {
            steps {
                dir('terraform') {
                    script {
                        withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: AWS_CREDENTIALS]]) {
                            sh 'terraform init'

                            def infraExists = sh(script: 'terraform state list | grep aws_eks_cluster.main', returnStatus: true) == 0
                            if (infraExists) {
                                echo 'Terraform infrastructure already exists. Skipping creation.'
                            } else {
                                echo 'Creating Terraform infrastructure...'
                                sh 'terraform apply -auto-approve'
                            }
                        }
                    }
                }
            }
        }

        stage('Build and Push Docker Images') {
            parallel {
                stage('Build & Push API Image') {
                    steps {
                        script {
                            def apiImage = "${DOCKER_REPOSITORY}-api:latest"
                            withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS, usernameVariable: 'DOCKERHUB_USERNAME', passwordVariable: 'DOCKERHUB_PASSWORD')]) {
                                retry(3) {
                                    sh """
                                        docker build -t ${apiImage} -f api/Dockerfile api
                                        echo $DOCKERHUB_PASSWORD | docker login -u $DOCKERHUB_USERNAME --password-stdin
                                        docker push ${apiImage}
                                    """
                                }
                            }
                        }
                    }
                }

                stage('Build & Push App Image') {
                    steps {
                        script {
                            def appImage = "${DOCKER_REPOSITORY}-app:latest"
                            withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS, usernameVariable: 'DOCKERHUB_USERNAME', passwordVariable: 'DOCKERHUB_PASSWORD')]) {
                                retry(3) {
                                    sh """
                                        docker build -t ${appImage} -f app/Dockerfile app
                                        echo $DOCKERHUB_PASSWORD | docker login -u $DOCKERHUB_USERNAME --password-stdin
                                        docker push ${appImage}
                                    """
                                }
                            }
                        }
                    }
                }

                stage('Build & Push DB Image') {
                    steps {
                        script {
                            def dbImage = "${DOCKER_REPOSITORY}-db:latest"
                            withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS, usernameVariable: 'DOCKERHUB_USERNAME', passwordVariable: 'DOCKERHUB_PASSWORD')]) {
                                retry(3) {
                                    sh """
                                        docker build -t ${dbImage} -f db/Dockerfile db
                                        echo $DOCKERHUB_PASSWORD | docker login -u $DOCKERHUB_USERNAME --password-stdin
                                        docker push ${dbImage}
                                    """
                                }
                            }
                        }
                    }
                }
            }
        }

        stage('Kubernetes: Deploy Application') {
            steps {
                script {
                    withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: AWS_CREDENTIALS]]) {
                        sh "aws eks --region ${AWS_REGION} update-kubeconfig --name ${EKS_CLUSTER_NAME}"

                        def workerNodesExist = sh(script: "kubectl get nodes | grep Ready", returnStatus: true) == 0
                        if (!workerNodesExist) {
                            error('No worker nodes available to deploy application. Verify node group configuration.')
                        }

                        echo 'Deploying application to Kubernetes...'
                        sh """
                            kubectl create namespace workshop || echo "Namespace already exists."

                            sed -i 's|image:.*backend.*|image: ${DOCKER_REPOSITORY}-api:latest|' k8s/backend/deployment.yaml
                            sed -i 's|image:.*frontend.*|image: ${DOCKER_REPOSITORY}-app:latest|' k8s/frontend/deployment.yaml
                            sed -i 's|image:.*database.*|image: ${DOCKER_REPOSITORY}-db:latest|' k8s/database/deployment.yaml

                            kubectl apply -f k8s/backend/ -n workshop
                            kubectl apply -f k8s/frontend/ -n workshop
                            kubectl apply -f k8s/database/ -n workshop
                        """
                    }
                }
            }
        }

        stage('Verify Deployment') {
            steps {
                script {
                    echo 'Verifying Kubernetes Deployment...'
                    sh """
                        kubectl get pods -n workshop
                        kubectl get svc -n workshop
                    """
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline executed successfully!'
        }
        failure {
            echo 'Pipeline failed. Check logs for more details.'
        }
    }
}
