# Real-Time Chat Service for CoHabit

## Overview
This real-time chat service allows two people to communicate about a specific post. Users can create conversations, send messages, and receive real-time updates using SignalR.

## Features
- ✅ One-to-one conversations between post owner and interested user
- ✅ Real-time message delivery using SignalR
- ✅ Message read status tracking
- ✅ Unread message count
- ✅ Typing indicators
- ✅ REST API fallback for non-real-time operations
- ✅ JWT authentication for both HTTP and WebSocket connections
- ✅ Conversation history with pagination
- ✅ User-friendly conversation list with last message preview

## Database Schema

### Conversation Table
- `ConversationId` (Guid, PK) - Unique identifier
- `PostId` (Guid, FK) - Reference to the post being discussed
- `User1Id` (Guid, FK) - Post owner
- `User2Id` (Guid, FK) - Interested user
- `CreatedAt` (DateTime) - When conversation was created
- `LastMessageAt` (DateTime?) - Timestamp of last message
- `IsActive` (bool) - Whether conversation is active

**Unique Constraint**: (PostId, User1Id, User2Id) - Ensures one conversation per post between two users

### Message Table
- `MessageId` (Guid, PK) - Unique identifier
- `ConversationId` (Guid, FK) - Reference to conversation
- `SenderId` (Guid, FK) - User who sent the message
- `Content` (string, max 2000) - Message content
- `IsRead` (bool) - Read status
- `CreatedAt` (DateTime) - When message was sent

**Indexes**: ConversationId, CreatedAt for efficient queries

## API Endpoints

### 1. Create or Get Conversation
```http
POST /api/chat/conversations
Authorization: Bearer {token}
Content-Type: application/json

{
  "postId": "guid"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Conversation created successfully",
  "data": {
    "conversationId": "guid",
    "postId": "guid",
    "postTitle": "string",
    "postImage": "string",
    "user1Id": "guid",
    "user1Name": "string",
    "user1Image": "string",
    "user2Id": "guid",
    "user2Name": "string",
    "user2Image": "string",
    "createdAt": "datetime",
    "lastMessageAt": "datetime",
    "lastMessage": "string",
    "unreadCount": 0,
    "isActive": true
  }
}
```

### 2. Get User Conversations
```http
GET /api/chat/conversations
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "message": "Conversations retrieved successfully",
  "data": [
    {
      "conversationId": "guid",
      "postId": "guid",
      "postTitle": "string",
      "postImage": "string",
      "user1Id": "guid",
      "user1Name": "string",
      "user1Image": "string",
      "user2Id": "guid",
      "user2Name": "string",
      "user2Image": "string",
      "createdAt": "datetime",
      "lastMessageAt": "datetime",
      "lastMessage": "string",
      "unreadCount": 2,
      "isActive": true
    }
  ]
}
```

### 3. Get Conversation Messages
```http
GET /api/chat/conversations/{conversationId}/messages?page=1
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "message": "Messages retrieved successfully",
  "data": [
    {
      "messageId": "guid",
      "conversationId": "guid",
      "senderId": "guid",
      "senderName": "string",
      "senderImage": "string",
      "content": "string",
      "isRead": true,
      "createdAt": "datetime"
    }
  ]
}
```

### 4. Send Message (REST API)
```http
POST /api/chat/messages
Authorization: Bearer {token}
Content-Type: application/json

{
  "conversationId": "guid",
  "content": "string"
}
```

### 5. Mark Messages as Read
```http
POST /api/chat/conversations/{conversationId}/read
Authorization: Bearer {token}
```

## SignalR Integration

### Connection
Connect to SignalR hub at: `wss://your-domain.com/hubs/chat`

**Authentication**: Pass JWT token via query string:
```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://your-domain.com/hubs/chat", {
    accessTokenFactory: () => accessToken
  })
  .build();
```

### Hub Methods (Client → Server)

#### 1. JoinConversation
Join a conversation to receive real-time messages:
```javascript
await connection.invoke("JoinConversation", conversationId);
```

#### 2. LeaveConversation
Leave a conversation:
```javascript
await connection.invoke("LeaveConversation", conversationId);
```

#### 3. SendMessage
Send a message in real-time:
```javascript
await connection.invoke("SendMessage", conversationId, messageContent);
```

#### 4. MarkMessagesAsRead
Mark messages as read:
```javascript
await connection.invoke("MarkMessagesAsRead", conversationId);
```

#### 5. UserTyping
Notify that user is typing:
```javascript
await connection.invoke("UserTyping", conversationId);
```

#### 6. UserStoppedTyping
Notify that user stopped typing:
```javascript
await connection.invoke("UserStoppedTyping", conversationId);
```

### Hub Events (Server → Client)

#### 1. ReceiveMessage
Triggered when a new message is sent:
```javascript
connection.on("ReceiveMessage", (message) => {
  console.log("New message:", message);
  // message structure matches MessageResponse
});
```

#### 2. NewMessage
Triggered for user-specific notifications:
```javascript
connection.on("NewMessage", (message) => {
  console.log("You have a new message:", message);
});
```

#### 3. NewMessageNotification
Triggered for other participants in conversation:
```javascript
connection.on("NewMessageNotification", (message) => {
  console.log("New message notification:", message);
  // Update unread count, show notification badge, etc.
});
```

#### 4. MessagesRead
Triggered when messages are marked as read:
```javascript
connection.on("MessagesRead", (conversationId, userId) => {
  console.log(`User ${userId} read messages in conversation ${conversationId}`);
  // Update UI to show read receipts
});
```

#### 5. UserTyping
Triggered when another user starts typing:
```javascript
connection.on("UserTyping", (conversationId, userId) => {
  console.log(`User ${userId} is typing in ${conversationId}`);
  // Show "User is typing..." indicator
});
```

#### 6. UserStoppedTyping
Triggered when another user stops typing:
```javascript
connection.on("UserStoppedTyping", (conversationId, userId) => {
  console.log(`User ${userId} stopped typing in ${conversationId}`);
  // Hide typing indicator
});
```

#### 7. ReceiveError
Triggered when an error occurs:
```javascript
connection.on("ReceiveError", (errorMessage) => {
  console.error("Error:", errorMessage);
  // Show error to user
});
```

## Frontend Integration Example

### React with SignalR

```javascript
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";

function ChatComponent({ conversationId, accessToken }) {
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState("");
  const [isTyping, setIsTyping] = useState(false);

  useEffect(() => {
    // Create SignalR connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://your-api.com/hubs/chat", {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    setConnection(newConnection);
  }, [accessToken]);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to SignalR");
          
          // Join conversation
          connection.invoke("JoinConversation", conversationId);

          // Listen for new messages
          connection.on("ReceiveMessage", (message) => {
            setMessages((prev) => [...prev, message]);
            
            // Mark as read if viewing this conversation
            if (message.senderId !== currentUserId) {
              connection.invoke("MarkMessagesAsRead", conversationId);
            }
          });

          // Listen for typing indicators
          connection.on("UserTyping", (convId, userId) => {
            if (convId === conversationId) {
              setIsTyping(true);
            }
          });

          connection.on("UserStoppedTyping", (convId, userId) => {
            if (convId === conversationId) {
              setIsTyping(false);
            }
          });

          // Listen for errors
          connection.on("ReceiveError", (error) => {
            console.error("SignalR Error:", error);
          });
        })
        .catch((error) => console.error("Connection failed:", error));

      return () => {
        connection.invoke("LeaveConversation", conversationId);
        connection.stop();
      };
    }
  }, [connection, conversationId]);

  const sendMessage = async () => {
    if (inputMessage.trim() && connection) {
      try {
        await connection.invoke("SendMessage", conversationId, inputMessage);
        setInputMessage("");
      } catch (error) {
        console.error("Failed to send message:", error);
      }
    }
  };

  const handleTyping = () => {
    if (connection) {
      connection.invoke("UserTyping", conversationId);
      
      // Send stopped typing after 1 second of no typing
      setTimeout(() => {
        connection.invoke("UserStoppedTyping", conversationId);
      }, 1000);
    }
  };

  return (
    <div className="chat-container">
      <div className="messages">
        {messages.map((msg) => (
          <div key={msg.messageId} className="message">
            <strong>{msg.senderName}:</strong> {msg.content}
          </div>
        ))}
        {isTyping && <div className="typing-indicator">User is typing...</div>}
      </div>
      <div className="input-area">
        <input
          type="text"
          value={inputMessage}
          onChange={(e) => {
            setInputMessage(e.target.value);
            handleTyping();
          }}
          onKeyPress={(e) => e.key === "Enter" && sendMessage()}
        />
        <button onClick={sendMessage}>Send</button>
      </div>
    </div>
  );
}
```

## Migration Commands

### Create Migration
```bash
dotnet ef migrations add AddChatFeature
```

### Apply Migration
```bash
dotnet ef database update
```

### Rollback Migration (if needed)
```bash
dotnet ef database update <PreviousMigrationName>
```

## Security Features

1. **JWT Authentication**: All endpoints and SignalR connections require valid JWT token
2. **User Verification**: Users can only:
   - Create conversations for posts they don't own
   - Access conversations they are part of
   - Send messages in their conversations
   - Read messages from their conversations
3. **Input Validation**: Message content limited to 2000 characters
4. **SQL Injection Prevention**: Entity Framework parameterized queries

## Business Rules

1. **One Conversation Per Post**: A post owner and interested user can only have one conversation per post
2. **No Self-Conversation**: Users cannot create conversations with themselves
3. **Two-Party Limit**: Each conversation is strictly between two users
4. **Message Ordering**: Messages are ordered by creation time
5. **Read Status**: Messages are marked as read when:
   - User views the conversation
   - User explicitly marks as read
6. **Soft Active Flag**: Conversations can be marked inactive without deletion

## Testing

### Test Create Conversation
```bash
curl -X POST https://localhost:7000/api/chat/conversations \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"postId":"POST_GUID"}'
```

### Test Send Message
```bash
curl -X POST https://localhost:7000/api/chat/messages \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"conversationId":"CONVERSATION_GUID","content":"Hello!"}'
```

## CORS Configuration

The application is already configured to accept SignalR connections from:
- `https://cohabit-dun.vercel.app`
- `http://localhost:5173`

Make sure to add any additional domains to the CORS policy in `Program.cs`.

## Performance Considerations

1. **Message Pagination**: Messages are paginated (50 per page) to reduce load
2. **Indexed Queries**: ConversationId and CreatedAt are indexed
3. **Efficient Updates**: LastMessageAt updated on each message
4. **Connection Management**: Automatic reconnection enabled
5. **Keep-Alive**: 15-second keep-alive interval prevents timeouts

## Troubleshooting

### SignalR Connection Issues
- Verify JWT token is valid and not expired
- Check CORS configuration includes your frontend domain
- Ensure WebSocket protocol is enabled on hosting server
- Check firewall settings allow WebSocket connections

### Message Not Delivered
- Verify user is part of the conversation
- Check if user has joined the conversation group
- Ensure SignalR connection is active
- Check server logs for errors

### Authentication Failures
- Token must be passed via `access_token` query parameter for SignalR
- Token must be in Authorization header for REST API
- Verify token hasn't expired

## Future Enhancements

- [ ] File/image sharing in messages
- [ ] Message editing and deletion
- [ ] Group conversations (more than 2 users)
- [ ] Voice/video call integration
- [ ] Message reactions (emoji)
- [ ] Search messages
- [ ] Conversation archiving
- [ ] Push notifications for mobile
- [ ] Message encryption

## Support

For issues or questions, please contact the development team or create an issue in the repository.
