const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/Chat/Chatter")
    .build();

document.getElementById("sendBtn").addEventListener("click", function () {
    let msg = document.getElementById("message").value;
    hubConnection.invoke("Send", msg)
        .catch(function (err) {
            return console.error(err.toString());
        });
});

hubConnection.on("Receive", function (message) {
    $.ajax({
        type: "POST",
        url: "/Chat/SendMessage",
        data: JSON.stringify({ message: message }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .done(function (response) {
            // Check if the response was successful
            if (response.success) {
                // Add the message to the chat room
                let chatroom = document.getElementById("chatroom");

                let divElem = document.createElement("div");
                divElem.setAttribute("class", "mess");
                divElem.style.background = "#f700ec";
                chatroom.appendChild(divElem);


                let messAuthor = document.createElement("div");
                messAuthor.setAttribute("class", "messAuthor");
                let imgAuthor = document.createElement("img");
                imgAuthor.setAttribute("src", decodeURI(response.ownerAvatar))
                let nameAuthor = document.createElement("h5");
                nameAuthor.innerText = response.ownerName;

                messAuthor.appendChild(imgAuthor);
                messAuthor.appendChild(nameAuthor);

                let messInfo = document.createElement("div");
                messInfo.setAttribute("class", "messInfo");
                let pText = document.createElement("p");
                pText.innerHTML = message;
                messInfo.appendChild(pText);

                divElem.appendChild(messAuthor);
                divElem.appendChild(messInfo);
                // Clear the message input field
                document.getElementById("message").value = "";
            }
        })
        .fail(function (xhr, status, error) {
            // Handle the error if needed
            console.log(error);
        });


});

hubConnection.start()
    .then(function () {
        document.getElementById("sendBtn").disabled = false;
    })
    .catch(function (err) {
        return console.error(err.toString());
    });