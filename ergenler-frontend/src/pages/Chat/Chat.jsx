import { useState, useEffect, useRef } from 'react';
import { useAuth } from '../../context/AuthContext';
import { chatService } from '../../services/api';
import * as signalR from '@microsoft/signalr';
import { FiSend, FiUser, FiSearch, FiMessageCircle } from 'react-icons/fi';
import './Chat.css';

export default function Chat() {
  const { user } = useAuth();
  const [chatList, setChatList] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [connection, setConnection] = useState(null);
  const [loading, setLoading] = useState(true);
  const messagesEndRef = useRef(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // Fetch Chat List
  useEffect(() => {
    const fetchChatList = async () => {
      try {
        const res = await chatService.getMyChatList();
        if (res.data?.isSuccess) {
          // Backend currently only returns Ids. If it returns Users, use that.
          // Fallback: show IDs if names are not available.
          const users = res.data.users || res.data.ids.map(id => ({ id, fullName: `Kullanıcı ${id.substring(0, 4)}` }));
          setChatList(users);
        }
      } catch (err) {
        console.error("Chat listesi alınamadı:", err);
      } finally {
        setLoading(false);
      }
    };
    fetchChatList();
  }, []);

  // SignalR Connection
  useEffect(() => {
    const token = localStorage.getItem('token');
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("/portal", {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log('SignalR Connected!');

          connection.on("ReceiveMessage", (message) => {
            // Yeni gelen mesaj başka birinden geldiği için isMine false olacak
            setMessages(prev => [...prev, {
              content: message.content,
              sendAt: message.sendAt,
              isMine: false
            }]);
          });

          connection.on("MarkAsRead", (senderId) => {
            console.log(`Messages from ${senderId} marked as read`);
          });

          connection.on("IsDelivered", (messageId) => {
            console.log(`Message ${messageId} delivered`);
          });
        })
        .catch(e => console.log('Connection failed: ', e));

      return () => {
        connection.stop();
      };
    }
  }, [connection, user]);

  // Fetch History when user selected
  useEffect(() => {
    if (selectedUser) {
      const fetchHistory = async () => {
        try {
          const res = await chatService.getMyChatHistory(selectedUser.id);
          if (res.data?.isSuccess) {
            // Sort messages by date ascending for the UI
            const sortedMessages = [...(res.data.messages || [])].sort((a, b) => 
              new Date(a.sendAt) - new Date(b.sendAt)
            );
            setMessages(sortedMessages);
            
            // Mark as read
            if (connection && connection.state === signalR.HubConnectionState.Connected) {
              await connection.invoke("MarkAsRead", selectedUser.id);
            }
          }
        } catch (err) {
          console.error("Mesaj geçmişi alınamadı:", err);
        }
      };
      fetchHistory();
    }
  }, [selectedUser, connection]);

  const handleSendMessage = async (e) => {
    e.preventDefault();
    if (!newMessage.trim() || !selectedUser || !connection) return;

    try {
      if (connection.state === signalR.HubConnectionState.Connected) {
        await connection.invoke("SendMessage", selectedUser.id, newMessage);
        
        // Add to local state immediately
        const newMsgObj = {
          content: newMessage,
          sendAt: new Date().toISOString(),
          isMine: true
        };
        setMessages(prev => [...prev, newMsgObj]);
        setNewMessage('');
      }
    } catch (err) {
      console.error("Mesaj gönderilemedi:", err);
    }
  };

  return (
    <div className="chat-container">
      <div className="chat-sidebar">
        <div className="chat-sidebar-header">
          <h3>Mesajlar</h3>
          <div className="chat-search">
            <input type="text" placeholder="Kişi ara..." />
            <FiSearch />
          </div>
        </div>
        <div className="chat-list">
          {loading ? (
            <div className="chat-loading">Yükleniyor...</div>
          ) : chatList.length === 0 ? (
            <div className="chat-empty">Henüz mesajınız yok.</div>
          ) : (
            chatList.map(u => (
              <div 
                key={u.id} 
                className={`chat-item ${selectedUser?.id === u.id ? 'active' : ''}`}
                onClick={() => setSelectedUser(u)}
              >
                <div className="chat-item-avatar">
                  <FiUser />
                </div>
                <div className="chat-item-info">
                  <div className="chat-item-name">{u.fullName || 'Bilinmeyen Kullanıcı'}</div>
                  <div className="chat-item-last">Son mesaj burada görünecek...</div>
                </div>
              </div>
            ))
          )}
        </div>
      </div>

      <div className="chat-main">
        {selectedUser ? (
          <>
            <div className="chat-header">
              <div className="chat-header-user">
                <div className="chat-header-avatar">
                  <FiUser />
                </div>
                <div>
                  <div className="chat-header-name">{selectedUser.fullName}</div>
                  <div className="chat-header-status">Çevrimiçi</div>
                </div>
              </div>
            </div>

            <div className="chat-messages">
              {messages.map((msg, index) => (
                <div 
                  key={index} 
                  className={`message-wrapper ${msg.isMine || msg.IsMine ? 'sent' : 'received'}`}
                >
                  <div className="message-content">
                    {msg.content}
                    <div className="message-time">
                      {new Date(msg.sendAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                    </div>
                  </div>
                </div>
              ))}
              <div ref={messagesEndRef} />
            </div>

            <form className="chat-input-area" onSubmit={handleSendMessage}>
              <input 
                type="text" 
                placeholder="Mesajınızı yazın..." 
                value={newMessage}
                onChange={(e) => setNewMessage(e.target.value)}
              />
              <button type="submit" className="send-btn" disabled={!newMessage.trim()}>
                <FiSend />
              </button>
            </form>
          </>
        ) : (
          <div className="chat-welcome">
            <FiMessageCircle size={48} />
            <h2>Sohbete Başla</h2>
            <p>Mesajlaşmak için soldaki listeden birini seçin.</p>
          </div>
        )}
      </div>
    </div>
  );
}
