# C# Authentication HTTP Requests

## Completed

I exported my Postman queries to a JSON file in the same level as this README. If you  want to use it you will need to update the bearer token on the collection itself, and on some of the queries (just check if they inherit or not).

## Create a complete webapi using Jwt Authentication

## Learning Objectives

- Use a bearer token to protect an API resource

## Introduction

In this exercise, you're going to build a small but complete bearer token authentication system. Here's a reminder for the flow:

1. The client sends a request containing login credentials to the server.
2. The server checks the credentials are correct and sends back a token.
3. The client sends a request to a secure resource, putting the token in the `authorization` header of the request.
4. The server verifies that the token is valid and sends back the requested resource, or an error message if the token wasn't valid.

Here's the diagram again to help you visualise this flow:

![](./assets/Auth_Flow.png)

### Recommended Domain

Create an API for a BlogPosts application:

`GET /posts` -> retrieve all blog post, requires any logged in user

`POST /posts` -> create a new post with the logged in user id

`PUT /posts` -> edit a blog post; **_, or the user that is the original author of the blog post_**

Recommended BlogPost model: id, text, authorId (note: this should be a string, to accommodate the UUID)

### Core Criteria

You will need to:

- Install relevant dependencies
- Add relevent configuration to the appsettings.json e.g. connection string to [neon](https://neon.tech) instance / token etc
- Add an endpoint which handles the Registration / Login
- Add entities, request/response models, db context, repository and any other classes you wish
- Keep code clean and comment where applicable using xml comments
- Add an endpoint which returns data from your database and ensure the endpoint method is decorated with the `[Authorize]` to secure it

### Extension Criteria

You should complete all of the above plus at least one of the following:

- Create endpoints for a user to
  - follow another user e.g. `POST user/1/follows/2`
  - unfollow another user e.g. `POST user/1/unfollows/2`
  - endpoint for a user to view all posts for users they are following e.g. `GET /viewall/1`
- Allow a user to comment on another post and show these in another endpoint like `GET /postswithcomments`
- Explore roles e.g. `[Authorize(Role="Administrator")]` and ensure that the post edit feature (aka. the PUT) can be edited by the author AND administrator.
