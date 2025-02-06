# FROM python:3.11-slim

# WORKDIR /app

# COPY . /app

# RUN pip install -r requirements.txt

# EXPOSE 5002 

# # 5002 this is for local dev

# CMD ["python", "app.py"]










FROM python:3.11-slim

# Set the working directory inside the container
WORKDIR /app

# Copy all application files
COPY . .

# Install dependencies directly from the source code (if needed)
RUN pip install --no-cache-dir Flask  # Add other dependencies manually if required

# Expose the Flask app's port
EXPOSE 5002

# Default command to run the Flask application
CMD ["python", "app.py"]







