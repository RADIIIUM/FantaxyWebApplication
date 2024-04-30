const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/Chat/Chatter")
    .build();

document.getElementById("sendBtn").addEventListener("click", function () {
    let message = document.getElementById("message").value;
    $.ajax({
        url: '/Chat/SendMessage',
        type: 'POST',
        contentType: 'application/json', // Add this line
        data: JSON.stringify({ message: message }),
        success: function (response) {
            hubConnection.invoke("Send", response)
                .catch(function (err) {
                    return console.error(err.toString());
                });
        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request
            console.error('Error sending message: ' + error);
        }
    });
});

hubConnection.on("Receive", function (response) {
    var ownAvatarUrl = document.getElementById("profileAva").src

    var chatroom = document.getElementById('chatroom');
    var messElement = document.createElement('div');
    messElement.classList.add('mess');
    messElement.style.background = (ownAvatarUrl === response.Avatar) ? '#f700ec' : '#420082';

    var messAuthorElement = document.createElement('div');
    messAuthorElement.classList.add('messAuthor');

    var avatarElement = document.createElement('img');
    avatarElement.src = response.Avatar;
    messAuthorElement.appendChild(avatarElement);

    var authorNameElement = document.createElement('h5');
    authorNameElement.textContent = response.Owner;
    messAuthorElement.appendChild(authorNameElement);

    messElement.appendChild(messAuthorElement);

    var messInfoElement = document.createElement('div');
    messInfoElement.classList.add('messInfo');

    var messageTextElement = document.createElement('p');
    messageTextElement.textContent = response.message;
    messInfoElement.appendChild(messageTextElement);

    messElement.appendChild(messInfoElement);

    chatroom.appendChild(messElement);
});

hubConnection.start()
    .then(function () {
        document.getElementById("sendBtn").disabled = false;
    })
    .catch(function (err) {
        return console.error(err.toString());
    });