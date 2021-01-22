# ProSeeker-
My defense project for ASP.NET Core MVC course at SoftUni (October 2020 @SoftUni)
## Brief description of the functionalities
<details>
   <summary>
       <strong> Click </strong> for more detailed information
   </summary>
ProSeeker‘s main idea is to be a platform based on supply and demand. A place where professionals in a certain field can be found by regular users who need their services. (mostly professionals who work on a recommendation basis).
Users themselves can directly seek a specialist or upload an ad and receive an offer for a service.

3 roles: regular user, specialist, administrator
User: 
- Can create/edit/delete Ad. 
- Can send inquiry to a professional and receive an offer.
- Can receive an offer from professional in two ways (from existing Ad or from sent inquiry).
- Has section with his ads only, where he can access each of them.
Specialist:
- Receive inquiries from regular users.
- Can make offers to clients (two ways : Ad/Inquiry).
Admin:
- Create/edit/delete job sub-categories;
- Create/edit/delete base job categories;
- Can create/edit/delete new surveys with questions and answers. When a certain user takes a survey, he becomes VIP for 1 week. 
VIP regular user – his ads will appear above all others, even after sorting criteria has been selected. If the user is specialist, his profile will appear above all others, even after sorting criteria has been selected. Each survey can be taken only once. 
Common actions for users and specialists:
- Both users and specialists are allowed to like specialists’ profiles, leave comments/opinions on users’ Ads and specialists’ profiles (recursively). 
- After accepting an offer, both parties receive emails with other person’s contacts.
- Update their profile info, add/change/delete avatar image.

Restrictions:
- Specialists can make only 1 offer to a certain Ad. If they try to send second offer to the same Ad, new modal window pops up and they can either cancel the attempt to make an offer or retrieve/delete the old offer and make a new one.
- Specialists can make more than 1 offers to regular user only when the user has sent an inquiry to the specialist (through the specialist profile).
- Guest users (not logged-in) are restricted to a very few actions.
- Only regular users can send an inquiry to specialists.
- Only specialists can send offers to regular users.
- Private chat is allowed for user-user / specialist-specialist. Users cannot start a private chat with specialists. This is against the main idea of the platform. 
- Server side + client side validations for all inputs.
</details>

## :hammer: Built With:
<details>
   <summary>
       <strong> Click </strong> for more detailed information
   </summary>

* <strong>.NET 5.0 <strong>
* <strong>Entity Framework Core 5.0 <strong> 
* <strong>FontAwesome<strong> (font icons)
* <strong>AutoMapper<strong> (object-to-object mapping library)
* <strong>Repository<strong> Pattern (Mainly for easier tests nad maintaining soft deletion)
* <strong>Cloudinary<strong> (file storage)
* <strong>TinyMCE<strong> (text redactor)
* <strong>HtmlSanitizer<strong> (XSS protection)
* <strong>Bootsrap 4<strong>
* <strong>JavaScript<strong> (well…)
* <strong>CSS<strong>
* <strong>HTML 5<strong>
* <strong>Moment.Js<strong> (JavaScript library for easier work with date-time)
* <strong>JQuery<strong>
* <strong>SignalR<strong> (used for real-time chat)
* <strong>WebAPI <strong>
* <strong>SendGrid<strong> (for sending emails) 
* <strong>xUnit<strong> (for testing) 

</details>

## DB Diagram
![](https://res.cloudinary.com/zmax/image/upload/v1609124986/81eec76a-fb6c-4ccf-9941-b4fe8bec34f9profilePicture.png)


## Unit tests (services test coverage)
![](https://res.cloudinary.com/zmax/image/upload/v1610213611/adfe08c8-bf3f-4958-bbc5-ea3f10ec67fcUnitTestsCoverage.png.png)
<!-- https://res.cloudinary.com/zmax/image/upload/v1610213611/adfe08c8-bf3f-4958-bbc5-ea3f10ec67fcUnitTestsCoverage.png.png -->

## :framed_picture: Image - Home Page

![](https://cdn1.bbcode0.com/uploads/2021/1/22/f79de7c1c4475756417f21beb7f1a53a-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/aa810dbbfbab64daa2085d84577889c0-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/2f02216a8f2e9703adeaaf5ef0816dbf-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/6be79ef9fe5eabbde6ce3d148e556580-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/32a97ddfa7e645a3e5d1eddab92b3b3e-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/289d6e825a946536890bdcb463452e6b-full.png)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/8b63ff03e673ae2b7954dc1375550eee-full.png)
![](https://bbcode0.com/uploaded/2021/1/22/919d024821dd425819e0d20e3ea418c2-full.png.html)
![](https://cdn1.bbcode0.com/uploads/2021/1/22/13cdf0505f282e42490e65c96d5921ff-full.png)