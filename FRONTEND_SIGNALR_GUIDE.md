# 🚀 Hướng Dẫn Kết Nối SignalR từ Frontend

## ⚠️ Lỗi CORS đã được fix!

Các thay đổi đã thực hiện:
1. ✅ Thêm hỗ trợ JWT authentication qua query string cho SignalR
2. ✅ Cấu hình CORS policy cho SignalR hub
3. ✅ Thêm `.RequireCors("AllowFrontend")` cho MapHub
4. ✅ Hỗ trợ cả WebSockets và LongPolling transport

## 📦 Cài Đặt Package

```bash
npm install @microsoft/signalr
# hoặc
yarn add @microsoft/signalr
```

## 🔧 Cách Kết Nối Đúng

### React/Next.js Example

```typescript
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";

function ChatComponent() {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [messages, setMessages] = useState<any[]>([]);
  
  // Lấy token từ cookie hoặc localStorage
  const getAccessToken = () => {
    // Option 1: Từ localStorage
    return localStorage.getItem("accessToken");
    
    // Option 2: Từ cookie
    // return document.cookie
    //   .split('; ')
    //   .find(row => row.startsWith('AccessToken='))
    //   ?.split('=')[1];
  };

  useEffect(() => {
    // Tạo connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://cohabit-api-c2b8h0gechbvfyap.southeastasia-01.azurewebsites.net/chathub", {
        accessTokenFactory: () => getAccessToken() || "",
        skipNegotiation: false, // Quan trọng: Phải false để negotiate
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        withCredentials: true // Quan trọng: Cho phép gửi credentials
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.elapsedMilliseconds < 60000) {
            return Math.random() * 10000;
          } else {
            return null;
          }
        }
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      // Bắt đầu kết nối
      connection
        .start()
        .then(() => {
          console.log("✅ Connected to SignalR!");
          
          // Listen for messages
          connection.on("ReceiveMessage", (message) => {
            console.log("📩 New message:", message);
            setMessages(prev => [...prev, message]);
          });

          connection.on("NewMessageNotification", (message) => {
            console.log("🔔 New message notification:", message);
            // Show notification
          });

          connection.on("UserTyping", (conversationId, userId) => {
            console.log(`⌨️ User ${userId} is typing...`);
          });

          connection.on("UserStoppedTyping", (conversationId, userId) => {
            console.log(`🛑 User ${userId} stopped typing`);
          });

          connection.on("MessagesRead", (conversationId, userId) => {
            console.log(`✓✓ Messages read by user ${userId}`);
          });

          connection.on("ReceiveError", (error) => {
            console.error("❌ SignalR Error:", error);
          });
        })
        .catch((error) => {
          console.error("❌ Connection failed:", error);
        });

      // Cleanup
      return () => {
        connection.stop();
      };
    }
  }, [connection]);

  // Join conversation
  const joinConversation = async (conversationId: string) => {
    if (connection) {
      try {
        await connection.invoke("JoinConversation", conversationId);
        console.log(`✅ Joined conversation: ${conversationId}`);
      } catch (error) {
        console.error("❌ Failed to join:", error);
      }
    }
  };

  // Send message
  const sendMessage = async (conversationId: string, content: string) => {
    if (connection) {
      try {
        await connection.invoke("SendMessage", conversationId, content);
        console.log("✅ Message sent!");
      } catch (error) {
        console.error("❌ Failed to send:", error);
      }
    }
  };

  // Mark as read
  const markAsRead = async (conversationId: string) => {
    if (connection) {
      try {
        await connection.invoke("MarkMessagesAsRead", conversationId);
        console.log("✅ Marked as read!");
      } catch (error) {
        console.error("❌ Failed to mark as read:", error);
      }
    }
  };

  // Typing indicator
  const notifyTyping = async (conversationId: string) => {
    if (connection) {
      try {
        await connection.invoke("UserTyping", conversationId);
      } catch (error) {
        console.error("❌ Failed to notify typing:", error);
      }
    }
  };

  return (
    <div>
      {/* Your chat UI */}
    </div>
  );
}

export default ChatComponent;
```

## 🔐 Authentication Options

### Option 1: Token từ localStorage (Recommended)
```typescript
.withUrl("YOUR_API_URL/chathub", {
  accessTokenFactory: () => localStorage.getItem("accessToken") || ""
})
```

### Option 2: Token từ Cookie
```typescript
.withUrl("YOUR_API_URL/chathub", {
  accessTokenFactory: () => {
    return document.cookie
      .split('; ')
      .find(row => row.startsWith('AccessToken='))
      ?.split('=')[1] || "";
  },
  withCredentials: true
})
```

### Option 3: Hardcode Token (Development only)
```typescript
.withUrl("YOUR_API_URL/chathub", {
  accessTokenFactory: () => "your-jwt-token-here"
})
```

## 🔍 Debugging CORS Issues

### 1. Kiểm tra Browser Console
```javascript
// Mở DevTools (F12) và chạy:
console.log("Cookies:", document.cookie);
console.log("Token:", localStorage.getItem("accessToken"));
```

### 2. Kiểm tra Network Tab
- Mở DevTools → Network
- Tìm request `/chathub/negotiate`
- Xem Headers:
  - Request Headers phải có `Origin: http://localhost:5173`
  - Response Headers phải có `Access-Control-Allow-Origin: http://localhost:5173`

### 3. Test Connection
```typescript
connection.start()
  .then(() => console.log("✅ Connected!"))
  .catch((err) => console.error("❌ Error:", err));
```

## 🚨 Common Errors & Solutions

### Error 1: "No 'Access-Control-Allow-Origin' header"
**Giải pháp:** Đảm bảo server đã restart sau khi update CORS config
```bash
# Stop và start lại API
dotnet run
```

### Error 2: "Failed to complete negotiation with the server"
**Giải pháp:** Kiểm tra JWT token có hợp lệ không
```typescript
const token = getAccessToken();
console.log("Token:", token);
console.log("Token expired?", isTokenExpired(token));
```

### Error 3: "WebSocket connection failed"
**Giải pháp:** Sử dụng LongPolling fallback
```typescript
.withUrl(url, {
  transport: signalR.HttpTransportType.LongPolling // Force LongPolling
})
```

### Error 4: 401 Unauthorized
**Giải pháp:** Token không đúng hoặc đã hết hạn
```typescript
// Refresh token trước khi connect
await refreshAccessToken();
const newToken = getAccessToken();
// Rồi mới connect
```

## 📱 Complete Chat Hook (React)

```typescript
import { useEffect, useState, useCallback } from "react";
import * as signalR from "@microsoft/signalr";

export const useChatHub = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [messages, setMessages] = useState<any[]>([]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://cohabit-api-c2b8h0gechbvfyap.southeastasia-01.azurewebsites.net/chathub", {
        accessTokenFactory: () => localStorage.getItem("accessToken") || "",
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    // Event handlers
    newConnection.on("ReceiveMessage", (message) => {
      setMessages(prev => [...prev, message]);
    });

    newConnection.onclose(() => setIsConnected(false));
    newConnection.onreconnecting(() => setIsConnected(false));
    newConnection.onreconnected(() => setIsConnected(true));

    newConnection.start()
      .then(() => {
        setIsConnected(true);
        console.log("✅ SignalR Connected");
      })
      .catch(err => console.error("❌ Connection error:", err));

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  const joinConversation = useCallback(async (conversationId: string) => {
    if (connection && isConnected) {
      await connection.invoke("JoinConversation", conversationId);
    }
  }, [connection, isConnected]);

  const sendMessage = useCallback(async (conversationId: string, content: string) => {
    if (connection && isConnected) {
      await connection.invoke("SendMessage", conversationId, content);
    }
  }, [connection, isConnected]);

  const leaveConversation = useCallback(async (conversationId: string) => {
    if (connection && isConnected) {
      await connection.invoke("LeaveConversation", conversationId);
    }
  }, [connection, isConnected]);

  const markAsRead = useCallback(async (conversationId: string) => {
    if (connection && isConnected) {
      await connection.invoke("MarkMessagesAsRead", conversationId);
    }
  }, [connection, isConnected]);

  return {
    connection,
    isConnected,
    messages,
    joinConversation,
    sendMessage,
    leaveConversation,
    markAsRead
  };
};
```

## ✅ Checklist Trước Khi Connect

- [ ] API đã được deploy và đang chạy
- [ ] JWT token hợp lệ và chưa hết hạn
- [ ] CORS policy đã được cấu hình đúng (đã fix trong code)
- [ ] Frontend origin đã được add vào CORS whitelist
- [ ] SignalR package đã được cài đặt
- [ ] Token được truyền đúng cách (localStorage hoặc cookie)
- [ ] Browser không block cookies (nếu dùng cookie)
- [ ] HTTPS được bật (production)

## 🌐 Production URLs

### Development
```typescript
const API_URL = "http://localhost:5000";
const HUB_URL = `${API_URL}/chathub`;
```

### Production
```typescript
const API_URL = "https://cohabit-api-c2b8h0gechbvfyap.southeastasia-01.azurewebsites.net";
const HUB_URL = `${API_URL}/chathub`;
```

## 📞 Support

Nếu vẫn gặp lỗi CORS, kiểm tra:
1. Server đã restart chưa?
2. Token còn hạn không?
3. Origin trong request có khớp với CORS policy không?
4. Browser console có log gì không?

**Lưu ý:** Sau khi update code, nhớ:
```bash
# Restart server
dotnet run

# Hoặc nếu đang deploy
git add .
git commit -m "Fix CORS for SignalR"
git push
```

Chúc bạn thành công! 🎉
