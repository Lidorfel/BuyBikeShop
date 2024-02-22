CREATE TABLE Products (
    id INT PRIMARY KEY IDENTITY(1,1),
    Class_Name VARCHAR(50),
    sub_class VARCHAR(50),
    title VARCHAR(100),
    description TEXT,
	price DECIMAL(10, 2),
    quantity INT,
    sale_perc INT,
    photo VARCHAR(255),
    color VARCHAR(50),
    release_date DATE,
    age_limit INT,
    rating DECIMAL(10, 2)
);