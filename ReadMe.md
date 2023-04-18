# TTVBot Documentation
TTVBot is a Telegram bot designed to provide text-to-voice and chat functionalities to users. It can convert written text into spoken messages, answer questions, and manage chat names and user nicknames.

## Features
- Text-to-voice conversion
- Responding to questions
- Renaming chat rooms and setting user nicknames
- Store and manage music samples
## Commands
* /ttv [text]: Converts the input text to voice and sends it as a voice message.
* /ask [question]: Asks a question, and the bot responds with a voice message.
* /cask [question]: Asks a concrete question, and the bot responds with a voice message.
* /chat [name]: Sets or updates the name of the chat.
* /name [nickname]: Sets or updates the user's nickname.
* /save [category]: Save sample in user or chat base.
* /show [category]: Save sample in user or chat base.
* /load [category] [name]: Save sample in user or chat base.
## Usage
Text-to-voice conversion
To use the text-to-voice feature, type the command /ttv followed by the text you want to convert:

```
/ttv Hello, how are you?
```
The bot will reply with a voice message containing the spoken version of the input text.

Asking questions
To ask a question, use the /ask command followed by your question:

```
/ask What is the capital of Kyiv?
```
The bot will reply with a voice message containing the answer to your question. If you want to ask a more concrete question (answer is **yes**, **no**, **maybe**), use the /cask command:

```
/cask IK22 the best
```
Renaming a chat and setting a nickname
To set or update the name of a chat, use the /chat command followed by the desired name:

```
/chat latestBack
```
To set or update your nickname, use the /name command followed by the desired nickname:

```
/name qwivet
```

# API Documentation

## Overview
This API provides functionality to interact with GPT-3 text generation and text-to-voice services and save data into MySQL database.

## GET /chat/name/{chatName}
Get the chat ID by its name.

### Parameters:

**chatName** (string, required) - _The name of the chat._

### Response:

**chatId** (integer) - _The chat ID._
## GET /chat/{chatId}
Get the chat name by its ID.

### Parameters:

**chatId** (integer, required) - _The ID of the chat._

### Response:

**chatName** (string) - _The chat name._
## POST /chat
Create a new chat in the database.

### Request Body:

**chatId** (integer, required) - _The chat ID._
**chatName** (string, required) - _The chat name._
## GET /user/name/{userName}
Get the user ID by its name.

### Parameters:

**userName** (string, required) - _The name of the user._

### Response:

**userId** (integer) - {The user ID.{
## GET /user/{userId}
Get the user name by its ID.

### Parameters:

**userId** (integer, required) - _The ID of the user._

### Response:

**userName** (string) - _The user name._
## POST /user
Create a new user in the database.

### Request Body:

**userId** (integer, required) - _The user ID._
**userName** (string, required) - _The user name._
## POST /gpt
Generate a response from GPT-4 and convert it to speech.

### Request Body:

**text** (string, required) - _The input text for GPT-3.5._
### Response:

_A stream containing the generated speech._
## POST /gpt/concrete
Generate a response from GPT-4 with a restricted answer format (Yes, No, Maybe) and convert it to speech.

### Request Body:

**text** (string, required) - _The input text for GPT-3.5._

### Response:

_A stream containing the generated speech._
## POST /ttv
Convert the input text to speech with an optional speed modifier.

### Request Body:

**text** (string, required) - _The input text for text-to-voice conversion._

### Response:

_A stream containing the generated speech._
## POST /ttv
Convert the input text to speech with an optional speed modifier.

### Request Body:

**text** (string, required) - _The input text for text-to-voice conversion._

### Response:

_A stream containing the generated speech._