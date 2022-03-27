# 288 CST Developer Test - Laurence Harker Solution

I have stuck to the brief and implemented the users stories provided, with a few embelishiments here and there. I didn't want to go too far off the User stories, given the suggested time for the work and the fact that this is not meant to be a fully functional app as it is part of a test.
My solution is targtting .Net Core 3.1 and useing an MVC pattern for the app, and couple of bits of Jquery, ajax, & javascript. It also makes use of a sqlserver database and EF to hold the stock records, and uses Moq in the tests.
The provided app is intneionally incomplete and one could see it as an agile Increment with the user stories ready for demo and setting as Done. 

## Enhancements

Within the scope of what I have compeleted were these additional things I considered:
Add support for multiple browser tabs (session id)
Fix discount code preservation once applied
Manage decrement of remaining stock numbers in the database
Accounts for users
Putting orders against users in database, providing an order history for each user
improve the css (what i've done is minimally acceptable, and i could play for days with it)
Log audit trail of actions in app to provide transaction history


## Things I have intentionally left out

User accounts and login
Mobile phone targeting 
Website caching
All browser support (chrome only)
Cookie management
Security certificates
Favicon
