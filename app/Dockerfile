# Use a lightweight Python image
FROM python:3.11-slim

# Set the working directory inside the container
WORKDIR /app

# Copy the application code
COPY . .

# Install dependencies (if you have a requirements.txt, uncomment the next line)
# RUN pip install --no-cache-dir -r requirements.txt

# Expose the Flask app's port
EXPOSE 5002

# Default command to run the Flask application
CMD ["python", "app.py"]
