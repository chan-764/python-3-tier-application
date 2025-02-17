-- Create database if it does not exist
CREATE DATABASE IF NOT EXISTS quotesdb;
USE quotesdb;

-- Create table for storing quotes
CREATE TABLE IF NOT EXISTS quotes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    content TEXT NOT NULL,
    author VARCHAR(255) NOT NULL
);

-- Insert sample data
INSERT INTO quotes (content, author) VALUES
('The best way to predict the future is to invent it.', 'Alan Kay'),
('Code is like humor. When you have to explain it, it’s bad.', 'Cory House'),
('Simplicity is the soul of efficiency.', 'Austin Freeman');
