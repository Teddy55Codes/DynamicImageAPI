# DynamicImageAPI

This api has different endpoints all based on the premise of dynamic images.

## Endpoints

### Images/Counter

#### parameters

- counterId

    default: `"0"`

    Is used to create multiple independent counters.

- fontSize
  
   default: `12`

   Set the fontsize for the response image.
  
- hexColor

   default: `"#000000"`

   Set the text color useful if you want to display the counter in a light and dark theme.`
  

#### description
Returns an image that displays a counter. 
Every time a request is made to a counter the counter will increment by 1.
There can be an almost infinite amount of counter because of the `counterId` parameter. 
Each counter has a separate count based on the `counterId`.

### Images/Location

#### parameters

- fontSize
   default: `12`

   Set the fontsize for the response image.


- hexColor
  default: `"#000000"`

  Set the text color useful if you want to display the counter in a light and dark theme.

#### description
Returns the name of the city from which the request was made as an image.
This information is fetched from ip-api.com. The specific request that is made is: `http://ip-api.com/json/1.1.1.1?fields=query,status,city,mobile,proxy`.
As of writing this the rate limit for this api is 45 requests per minute.
The location data is cleared every 24h. This is done to always have fresh location data. 
