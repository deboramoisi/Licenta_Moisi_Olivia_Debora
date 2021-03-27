class Message {
    constructor(username, text, when) {
        this.userName = username;
        this.text = text;
        this.when = when;
    }
}

// userName is declared in razor page.
const username = userName;
const textInput = document.getElementById('messageText');
const timeInput = document.getElementById('when');
const chat = document.getElementById('chat');
const messagesQueue = [];

// la Click pe butonul de submit
document.getElementById('submitButton').addEventListener('click', () => {
    var currentdate = new Date();
    let when = new Date();
    when.innerHTML =
        (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear()
});

// pentru incepere intrare in chat, golim field-ul
function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
}

// pentru trimitere mesaj
function sendMessage() {
    let text = messagesQueue.shift() || "";
    if (text.trim() === "") return;

    let message = new Message(username, text);
    sendMessageToHub(message);
}

// Adaugare mesaj in hub, display for everyone
function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let container = document.createElement('div');
    // stilizare pentru UI
    container.className = isCurrentUserMessage ? "container darker" : "container";

    // appending all the data
    let sender = document.createElement('p');
    sender.className = "sender";
    sender.innerHTML = message.userName;
    let text = document.createElement('p');
    text.innerHTML = message.text;

    let when = document.createElement('span');
    when.className = isCurrentUserMessage ? "time-left" : "time-right";
    var currentdate = new Date();
    when.innerHTML =
        (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear()

    container.appendChild(sender);
    container.appendChild(text);
    container.appendChild(when);
    chat.appendChild(container);
}
