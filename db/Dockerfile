# Use the official MySQL image
FROM mysql:latest

# Set environment variables for MySQL
ENV MYSQL_DATABASE=quotesdb \
    MYSQL_ROOT_PASSWORD=root \
    MYSQL_USER=user \
    MYSQL_PASSWORD=password

# Copy initialization SQL script to the container
COPY init.sql /docker-entrypoint-initdb.d/

# Expose MySQL port
EXPOSE 3306
