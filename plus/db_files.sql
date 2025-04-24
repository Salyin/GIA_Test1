DROP TABLE IF EXISTS grocery_user CASCADE;
DROP TABLE IF EXISTS grocery_category CASCADE;
DROP TABLE IF EXISTS grocery_item CASCADE;
DROP TABLE IF EXISTS grocery_cart_item CASCADE;
DROP TABLE IF EXISTS grocery_image CASCADE;

CREATE TABLE grocery_image(
	id SERIAL PRIMARY KEY,
	image_path VARCHAR(128) NOT NULL
);

CREATE TABLE grocery_user(
	id SERIAL PRIMARY KEY,
	user_name VARCHAR(256) NOT NULL,
	user_password VARCHAR(256) NOT NULL
);

CREATE TABLE grocery_category(
	id SERIAL PRIMARY KEY,
	category_name VARCHAR(64) NOT NULL,
	image_id INT NOT NULL
);

CREATE TABLE grocery_item(
	id SERIAL PRIMARY KEY,
	item_category INT REFERENCES grocery_category(id),
	item_image INT REFERENCES grocery_image(id),
	item_name VARCHAR(256) NOT NULL,
	item_desc VARCHAR(128) NOT NULL,
    item_detail VARCHAR(512) NOT NULL,
	price MONEY NOT NULL
);

CREATE TABLE grocery_cart_item(
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES grocery_user(id),
    item_id INT REFERENCES grocery_item(id),
    item_quantity INT NOT NULL
);

INSERT INTO grocery_image (image_path) VALUES
	('/Materials/Bakery.png'),
	('/Materials/Bananas.png'),
	('/Materials/CocaCola.png'),
	('/Materials/DietCoke.png'),
	('/Materials/Drinks.png'),
	('/Materials/Eggs.png'),
	('/Materials/FruitsAndVeaetables.png'),
	('/Materials/REdApple.png'),
	('/Materials/NotFound.jpg'),
	('/Materials/Bananas.png'),
	('/Materials/RedPepper.png'),
	('/Materials/OilOne.png'),
	('/Materials/Eggs.png'),
	('/Materials/TreeTopPineapple.png'),
	('/Materials/TreeTopAppleGrape.png'),
	('/Materials/Pepsi.png'),
	('/Materials/Sprite.png');

INSERT INTO grocery_category (category_name, image_id) VALUES
	('Fruits and Vegetables', 101),
	('Oil', 102),
	('Meat and Fish', 103),
	('Milk and Eggs', 104),
	('Beverages', 105),
	('Bakery', 106);

INSERT INTO grocery_item (item_category, item_image, item_name, item_desc, item_detail, price) VALUES
(1, 8, 'Red Apple', 'Fresh red apple', 'Crisp and juicy red apples from local farms', 0.80),
(1, 9, 'Horseradish', 'Spicy horseradish root', 'Freshly harvested horseradish root, pungent and full of flavor', 1.20),
(1, 10, 'Bananas', 'Ripe bananas', 'Sweet and nutritious bananas rich in potassium', 0.60),
(1, 11, 'Red Bell Pepper', 'Crunchy red bell pepper', 'Bright red bell peppers, sweet and crisp, great for salads', 1.00),

(2, 12, 'Sunflower Oil', 'Refined sunflower oil', '1L bottle of refined sunflower oil for cooking and frying', 3.50),

(3, 9, 'Chicken Breast', 'Boneless chicken breast', 'Fresh, skinless and boneless chicken breast, ideal for grilling or baking', 5.25),

(4, 13, 'Eggs', 'Farm fresh eggs', '12-pack of organic free-range chicken eggs', 2.50),

(5, 14, 'TreeTop Pineapple', 'Pineapple flavored drink', 'Refreshing pineapple juice drink from TreeTop', 1.75),
(5, 15, 'TreeTop Apple Grape', 'Apple-grape flavored drink', 'Delicious blend of apple and grape juice from TreeTop', 1.75),
(5, 16, 'Pepsi', 'Carbonated soft drink', 'Classic Pepsi cola in 0.5L bottle', 1.50),
(5, 3, 'Coca-Cola', 'Carbonated soft drink', 'Original taste Coca-Cola, chilled and fizzy', 1.50),
(5, 17, 'Sprite', 'Lemon-lime soda', 'Crisp and refreshing Sprite in 0.5L bottle', 1.50),
(5, 4, 'Diet Coke', 'Low-calorie cola drink', 'Smooth taste with less sugar, 0.5L bottle', 1.50),

(6, 9, 'Sourdough Bread', 'Artisan sourdough loaf', 'Crusty on the outside, soft on the inside, slow-fermented sourdough', 2.80);
