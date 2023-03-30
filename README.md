# Warehouse Microservice 🏭
This microservice is designed to provide easy access to product information stored in the warehouse through an intuitive API. Its main features include:

1. **Data Storage:** The microservice stores all product information, including descriptions, images, and other characteristics.
2. **Querying:** Users can query the microservice to retrieve information about specific products, such as their name, description, and image.
3. **Editing:** The API allows users to edit existing product information or add new products to the database.
4. **Robustness:** The microservice is designed to be robust and scalable, ensuring that it can handle large volumes of requests without any issues.
 
Overall, the goal of this project is to provide a comprehensive and easy-to-use API for managing all aspects of product information in the warehouse.

Table of Contents
=================
* [Data formats:](#data-formats)
    * [Brand](#brand-) 🛍️
        * [Brand Response](#brand-response)
        * [Brand Request](#brand-request)
    * [Category](#category-) 📦
        * [Category Request](#category-request)
        * [Category Response](#category-response)
    * [Provider](#provider-) 🚚 
        * [Provider Request](#provider-request)
        * [Provider Response](#provider-response)
    * [Product](#product-) 📝
        * [Product Request](#product-request)
        * [Product Response](#product-response)
    * [Image](#image-) 🖼️
        * [Image file response](#image-file-response)
* [API endpoints](#api-endpoints)
    * [Brands](#brands) 🛍️
        * [GET /api/brands/all](#get-apibrandsall)
        * [GET /api/brands/{id}](#get-apibrandsid)
        * [POST /api/brands](#post-apibrands)
        * [PUT /api/brands/{id}](#put-apibrandsid)
        * [DELETE /api/brands/{id}](#delete-apibrandsid)
    * [Categories](#categories) 📦
        * [GET /api/categories/all](#get-apicategoriesall)
        * [GET /api/categories/{id}](#get-apicategoriesid)
        * [POST /api/categories](#post-apicategories)
        * [PUT /api/categories/{id}](#put-apicategoriesid)
        * [DELETE /api/categories/{id}](#delete-apicategoriesid)
    * [Providers](#providers) 🚚 
        * [GET /api/providers/all](#get-apiprovidersall)
        * [GET /api/providers/{id}](#get-apiprovidersid)
        * [POST /api/providers](#post-apiproviders)
        * [PUT /api/providers/{id}](#put-apiprovidersid)
        * [DELETE /api/providers/{id}](#delete-apiprovidersid)
    * [Products](#products) 📝
        * [GET /api/products/all](#get-apiproductsall)
        * [GET /api/products/{id}](#get-apiproductsid)
        * [POST /api/products](#post-apiproducts)
        * [PUT /api/products/{id}](#put-apiproductsid)
        * [DELETE /api/products/{id}](#delete-apiproductsid)
    * [Images](#images) 🖼️
      * [POST /api/images](#post-apiimages)

This microservice is responsible for storing, adding, editing and fetching information about products and their characteristics.

The main goal of this project is to make an easy-to-use API for querying and changing all kinds of
information about products, stored in the warehouse.
# Data formats

## Brand 🛍️

### Brand Response
### Brand Request
Represents the data used to update an existing or create a new one brand with the following properties:

* name (string, required) - the name of the brand
* image (string, required) - the URL of an image representing the brand
* description (string, required) - the brief description of the brand

Example:
```json
{
  "name": "Sad Socks",
  "image": "https://sadsockslogo.svg",
  "description": "These so socks are so sad. You will cry really hard because of it:("
}
```

Represents a brand object with the following properties:

* brandId (string) - the unique identifier of the brand
* name (string) - the name of the brand
* image (string) - the URL of an image representing the brand
* description (string) - a brief description of the brand

Example:
```json
{
  "brandId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
  "name": "Happy Socks",
  "image": "https://happysockslogo.svg",
  "description": "This so socks are so happy. You won't believe it!"
}
```

## Category 📦
### Category Request

Represents the data used to update an existing category or create a new one with the following properties:

* Name (string, required) - the name of the category

Example:
```json
{
  "Name": "Men's Socks"
}
```

### Category Response
Represents a category object with the following properties:

* CategoryId (string) - the unique identifier of the category
* Name (string) - the name of the category
 
Example:
```json
{
  "categoryId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
  "name": "Men's Socks"
}
```

## Provider 🚚 
### Provider Request
Represents the data used to update an existing provider or create a new one with the following properties:

* companyName (string, required) - the name of the provider's company
* phoneNumber (string, required) - the phone number of the provider
* email (string, required) - the email address of the provider

Example:
```json
{
  "companyName": "ACME Inc.",
  "phoneNumber": "+1-555-123-4567",
  "email": "contact@acme.com"
}
```

### Provider Response
Represents a provider object with the following properties:

* providerId (string) - the unique identifier of the provider
* companyName (string) - the name of the provider's company
* phoneNumber (string) - the phone number of the provider
* email (string) - the email address of the provider

Example:
```json
{
  "providerId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
  "companyName": "ACME Inc.",
  "phoneNumber": "+1-555-123-4567",
  "email": "contact@acme.com"
}
```

## Product 📝
### Product Request
Represents the data used to update an existing product or create a new one with the following properties:

* name (string, required) - the name of the product
* quantity (integer, required) - the quantity of the product
* fullPrice (decimal, required) - the full price of the product
* description (string, required) - the description of the product
* mainImage (string, required) - URL of main image of the product
* images(required) - array of images of the product. The main image must not be included here
* sale (decimal, required) - the sale percentage of the product(if product is on sale this value should be 0)
* isActive (boolean, required) - a flag indicating whether the product is currently active
* categoryId (string, required) - the ID of the category the product belongs to
* providerId (string, required) - the ID of the provider that supplies the product
* brandId (string, required) - the ID of the brand of the product
 
Example:

```json
{
  "name": "Sad Socks",
  "quantity": 50,
  "fullPrice": 10.5,
  "description": "These so socks are so sad. You will cry really hard because of it:(",
  "mainImage": "https://happysocksmainimage.jpg",
  "images": [
    "https://happysocksimage1.svg",
    "https://happysocksimage2.svg"
  ],
  "sale": 9.0,
  "isActive": true,
  "categoryId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
  "providerId": "d80ba9a5-6a48-4aa5-b31b-07ad0564a4ee",
  "brandId": "f21fa9bb-ec6b-46db-a17f-cb298c96d1a1"
}
```

### Product Response
Represents a product object with the following properties:


* ProductId (guid, required) - the unique identifier of the product
* Name (string, required) - the name of the product
* Quantity (integer, required) - the quantity of the product
* FullPrice (decimal, required) - the full price of the product
* Description (string, required) - the description of the product
* MainImage (string, required) - URL of main image of the product
* Images(required) - array of images of the product. The main image must not be included here
* Sale (decimal, required) - the sale percentage of the product(if product is on sale this value shouuld be 0)
* IsActive (boolean, required) - a flag indicating whether the product is currently active
* Category (string) - the name of the category that the product belongs to
* Provider ([Provider Request](#provider-request)) - the provider of the product
* Brand ([Brand Request](#brand-request)) - the brand of the product

Example:
```json
{
    "productId": "1c2e87b6-0e31-4d8e-af50-1f0e93f06c0d",
    "name": "Happy Socks",
    "quantity": 10,
    "fullPrice": 15.00,
    "description": "These socks are so happy. You won't believe it!",
    "mainImage": "https://happysocksmainimage.jpg",
    "images": [
        "https://happysocksimage1.svg",
        "https://happysocksimage2.svg"
    ],
    "sale": 12.50,
    "isActive": true,
    "category": "Men's Socks",
    "provider": {
        "companyName": "Happy Socks Co.",
        "phoneNumber": "555-1234",
        "email": "contact@happysocks.com"
    },
    "brand": {
        "name": "Happy Socks",
        "image": "https://happysockslogo.svg",
        "description": "This so socks are so happy. You won't believe it!"
    }
}
```
## Image 🖼️
### Image File Response
Represents an image file object with the following properties:

- FileName (string) - the original name of the uploaded image file

- FileUrl (string) - the URL of the uploaded image file on ImageKit, 
 which provides real-time image transformations through URL parameters. 
 More information about ImageKit transformations can be found 
 in their [documentation](https://docs.imagekit.io/features/image-transformations)).
 
Example:
```json
{
  "FileName": "example_image.jpg",
  "FileUrl": "https://ik.imagekit.io/v3vfqohwz/4180314f-3879-4e1c-a0d6-86e44c4a0a42.png"
}
```
# API endpoints

## Brands

### `GET /api/brands/all`

Returns all the brands in the warehouse.

***Query Parameters:***

- pageIndex (optional, default = 1) - The page number to retrieve.
- pageSize (optional, default = 15) - The number of records per page.
 
***Status codes:***
* 200 OK - The request was successful and the response contains an array of brands.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Response body:***
```json
[
    {
      "brandId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
      "name": "Happy Socks",
      "image": "https://happysockslogo.svg",
      "description": "These so socks are so happy. You won't believe it!"
    },
  
    {
      "brandId": "0b276958-258b-48d1-ada1-25f418240f37",
      "name": "Happy cocks",
      "image": "https://happycockslogo.jpg",
      "description": "These cocks are really happy to be eaten by you. Exactly you."
    }
]
```

### `GET /api/brands/{id}`

Returns a single brand by its GUID id.

***Status codes:***

* 200 OK - The request was successful and the response contains a single brand object.
* 404 Not Found - The brand with the specified id could not be found.
* 500 Internal Server Error - The server encountered an error while processing the request.

***Response body:***
* [Brand Response](#brand-response)

### `POST /api/brands`

***Status codes:***

* 201 Created - The brand was successfully created and the response contains the new brand object.
* 400 Bad Request - The request was malformed or missing required fields.
* 500 Internal Server Error - The server encountered an error while processing the request.

***Request body:***

* [Brand Request](#brand-request)


***Response body:***

* [Brand Response](#brand-response)

### `PUT /api/brands/{id}`

***Status codes:***

* 204 No Content on success
* 400 Bad Request if the request body is invalid
* 404 Not Found if the brand with the specified ID was not found
* 500 Internal Server Error - The server encountered an error while processing the request.
 
 
***Request body:***

* [Brand Request](#brand-request)

### `DELETE /api/brands/{id}`

| ⚠️ WARNING                                                                      |
|:--------------------------------------------------------------------------------|
| When deleting a brand all the products, associated with it will also be deleted |
***Status codes:***

* 204 The brand was successfully deleted. No content in the response body.
* 404 The brand with the specified ID was not found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
## Categories

### `GET /api/categories/all`
Returns all the categories in the warehouse.

***Query Parameters:***

- pageIndex (optional, default = 1) - The page number to retrieve.
- pageSize (optional, default = 15) - The number of records per page.

***Status codes:***
* 200 OK - The request was successful and the response contains an array of categories.
* 500 Internal Server Error - The server encountered an error while processing the request.

***Response body:***
```json
[
    {
        "categoryId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
        "name": "Electronics",
        "description": "Devices that use electricity to perform a specific function."
    },
    {
        "categoryId": "0b276958-258b-48d1-ada1-25f418240f37",
        "name": "Home Goods",
        "description": "Products for use in and around the home."
    }
]
```
### `GET /api/categories/{id}`
Returns a single category by its GUID id.

***Status codes:***

* 200 OK - The request was successful and the response contains a single category object.
* 404 Not Found - The category with the specified id could not be found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Response body:***

[Category Response](#category-response)

### `POST /api/categories`
Creates a new category.

***Status codes:***

* 201 Created - The category was successfully created and the response contains the new category object.
* 400 Bad Request - The request was malformed or missing required fields.
* 500 Internal Server Error - The server encountered an error while processing the request.

***Request body:***

[Category Request](#category-response)

***Response body:***

[Category Response](#category-response)


### `PUT /api/categories/{id}`
Updates a provider by its GUID id.

***Status codes:***

* 204 No Content on success
* 400 Bad Request if the request body is invalid
* 404 Not Found if the provider with the specified ID was not found
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Request body:***

[Category Request](#category-response)
### `DELETE /api/categories/{id}`
Deletes a category by its GUID id.

| ⚠️ WARNING                                                                         |
|:-----------------------------------------------------------------------------------|
| When deleting a category all the products, associated with it will also be deleted |


***Status codes:***

* 204 The category was successfully deleted. No content in the response body.
* 404 The category with the specified ID was not found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
## Providers
### `GET /api/providers/all`

Returns all the providers in the warehouse.

***Query Parameters:***

- pageIndex (optional, default = 1) - The page number to retrieve.
- pageSize (optional, default = 15) - The number of records per page.
 
***Status codes:***
* 200 OK - The request was successful and the response contains an array of provider objects.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Response body:***
```json
[
    {
        "providerId": "61a0dcde-79c0-4240-abde-dfb9f68a8ef6",
        "name": "Acme Inc.",
        "address": "123 Main St.",
        "phone": "555-1234",
        "email": "acme@example.com"
    },
  
    {
        "providerId": "0b276958-258b-48d1-ada1-25f418240f37",
        "name": "Globex Corporation",
        "address": "456 Broadway",
        "phone": "555-5678",
        "email": "globex@example.com"
    }
]
```

### `GET /api/providers/{id}`

Returns a single provider by its GUID id.

***Status codes:***

* 200 OK - The request was successful and the response contains a single provider object.
* 404 Not Found - The provider with the specified id could not be found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Response body:***

[Provider response](#provider-response)

### `POST /api/providers`

Creates a new provider.

***Status codes:***

* 201 Created - The provider was successfully created and the response contains the new provider object.
* 400 Bad Request - The request was malformed or missing required fields.
* 500 Internal Server Error - The server encountered an error while processing the request.
  
***Request body:***

[Provider request](#provider-request)

***Response body:***

[Provider response](#provider-response)

### `PUT /api/providers/{id}`

***Status codes:***

* 204 No Content on success
* 400 Bad Request if the request body is invalid
* 404 Not Found if the provider with the specified ID was not found
* 500 Internal Server Error - The server encountered an error while processing the request.
 
Updates a provider by its GUID id.

***Request body:***

[Provider request](#provider-request)

### `DELETE /api/providers/{id}`
Deletes a provider by its GUID id.

| ⚠️ WARNING                                                                         |
|:-----------------------------------------------------------------------------------|
| When deleting a provider all the products, associated with it will also be deleted |

***Status codes:***

* 204 The provider was successfully deleted. No content in the response body.
* 404 The provider with the specified ID was not found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
## Products
### `GET /api/products/all`
Returns all the products in the warehouse.

***Query Parameters:***

- pageIndex (optional, default = 1) - The page number to retrieve.
- pageSize (optional, default = 15) - The number of records per page.
 
***Status codes:***

* 200 OK - The request was successful and the response contains an array of products.
* 500 Internal Server Error - The server encountered an error while processing the request.

***Response body:***
 
```json
[
  {
    "productId": "1c2e87b6-0e31-4d8e-af50-1f0e93f06c0d",
    "name": "White happy socks for men",
    "quantity": 10,
    "fullPrice": 15.00,
    "description": "These socks are so happy. You won't believe it!",
    "mainImage": "https://happysocksmainimage.png",
    "images": [
        "https://happysocksimage1.svg",
        "https://happysocksimage2.svg"
    ],
    "sale": 12.50,
    "isActive": true,
    "category": "Women's Socks",
    "provider": {
      "companyName": "Happy Socks Co.",
      "phoneNumber": "555-1234",
      "email": "contact@happysocks.com"
    },
    "brand": {
      "name": "Happy Socks",
      "image": "https://happysockslogo.svg",
      "description": "This so socks are so happy. You won't believe it!"
    }
  },

  {
    "productId": "1c2e87b6-0e31-4d8e-af50-1f0e93f06c0d",
    "name": "Black sad socks for women",
    "quantity": 10,
    "fullPrice": 15.00,
    "description": "These socks are so sad. They will make you cry!",
    "mainImage": "https://sadsocksmainimage.png",
    "images": [
        "https://sadsocksimage1.svg",
        "https://sadsocksimage2.svg"
    ],
    "sale": 50,
    "isActive": true,
    "category": "Women's Socks",
    "provider": {
      "companyName": "Happy Socks Co.",
      "phoneNumber": "555-1234",
      "email": "contact@sadsocks.com"
    },
    "brand": {
      "name": "Happy Socks",
      "image": "https://happysockslogo.svg",
      "description": "This so socks are so happy. You won't believe it!"
    }
  }
]
```
### `GET /api/products/{id}`
Returns a single product by its GUID id.

***Status codes:***

* 200 OK - The request was successful and the response contains a single product object.
* 404 Not Found - The product with the specified id could not be found.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Response body:***

[Product response](#product-response)

### `POST /api/products`

***Status codes:***

* 201 Created - The product was successfully created and the response contains the new product object.
* 400 Bad Request - The request was malformed or missing required fields.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Request body:***

[Product Request](#product-request)

***Response body:***

[Product response](#product-response)

### `PUT /api/products/{id}`

***Status codes:***

* 204 No Content on success
* 400 Bad Request if the request body is invalid
* 404 Not Found if the product with the specified ID was not found
* 500 Internal Server Error - The server encountered an error while processing the request.

***Request body:***

[Product Request](#product-request)

### `DELETE /api/products/{id}`

***Status codes:***

* 204 The product was successfully deleted. No content in the response body.
* 404 The product with the specified ID
* 500 Internal Server Error - The server encountered an error while processing the request.

## Images
This endpoint is used for uploading images and obtaining their URLs, which can then be used in other endpoints. 

### `POST /api/images`
Uploads images and returns a list of their corresponding URLs.

***Status codes:***

* 200 OK - The request was successful, and the response contains an array of ImageFileDto objects with the file names and URLs.
* 400 Bad Request - The request was malformed or missing required fields.
* 500 Internal Server Error - The server encountered an error while processing the request.
 
***Request body:***

A multipart/form-data request containing an array of images. The allowed image content types are:

- image/jpeg
- image/png
- image/webp
- image/svg+xml
 
***Response body:***

List of [Image file response](#image-file-response)
```json
[
  {
    "FileName": "example_image1.jpg",
    "FileUrl": "https://your-imagekit-url.example.com/example_image1.jpg"
  },
  
  {
    "FileName": "example_image2.jpg",
    "FileUrl": "https://your-imagekit-url.example.com/example_image2.jpg"
  }
]
```
