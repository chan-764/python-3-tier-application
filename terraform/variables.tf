variable "vpc_cidr" {
  description = "CIDR block for the VPC"
  type        = string
}

variable "subnet_1_cidr" {
  description = "CIDR block for subnet 1"
  type        = string
}

variable "subnet_2_cidr" {
  description = "CIDR block for subnet 2"
  type        = string
}

variable "az_1" {
  description = "Availability Zone for subnet 1"
  type        = string
}

variable "az_2" {
  description = "Availability Zone for subnet 2"
  type        = string
}

variable "ami_id" {
  description = "AMI ID for EC2 instances"
  type        = string
  default     = "ami-04c0ab8f1251f1600" # Latest Amazon Linux 2 AMI for us-west-2
}

variable "instance_type" {
  description = "Instance type for EC2"
  type        = string
  default     = "t2.micro"
}
variable "nlb_name" {
  description = "Name of the Network Load Balancer"
  type        = string
  default     = "my-nlb"
}

variable "tg_name" {
  description = "Name of the Target Group"
  type        = string
  default     = "my-target-group"
}
