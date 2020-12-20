"use strict";

(function () {
    updateScroll();
}());

var connection = new signalR.HubConnectionBuilder().withUrl("/privateChatHub").build();
connection.start().catch(function (err) {
    return console.error(err.toString());
});
//<p class="small text-muted"> AAAA ${@Model.Sender.FirstName}</p>
connection.on("SendMessage",
    function (message) {
        let currentUsername = $('#currentUserUserName').text();
        let escapedMessage = escapeHtml(message.content);
        let chatInfo = '';

        if (currentUsername != message.senderUserName) {

            let formattedDateTime = formatDateTime();

            chatInfo =
                `<div class="media col-md-7 ml-auto mb-1 mr-5">
                 <div class="media-body d-inline">
                   <p class="text-small custom-receiver-chat-content mr-3">
                       ${escapedMessage}
                       <time class="small ml-1 my-message-date float-right pt-0" datetime="${message.createdOn.toString("O")}" style="color:darkgray;">${formattedDateTime}</time>
                   </p>
                 </div>
                </div>`
        }

        $("#messageContainer").append(chatInfo);
        updateScroll();
    });

function displayMessage(message) {
    let formattedDateTime = formatDateTime();

    let chatInfo = ` <div class="media col-md-7 mb-1">
                <div class="media-body ml-4">
                    <p class="text-small mb-0 custom-sender-chat-content">
                        ${message}
                        <time class="small  my-message-date float-right" datetime="${formattedDateTime}" style="color:darkgray">${formattedDateTime}</time>
                    </p>
                </div>
            </div>`
    $("#messageContainer").append(chatInfo);
}

$("#sendButton").click(function (e) {
    e.preventDefault();
    let conversationId = $('#hConversationId').val();
    let senderId = $('#hSenderId').val();
    let receiverId = $('#hReceiverId').val();
    let message = $("#messageInput").val();

    displayMessage(escapeHtml(message));
    connection.invoke("Send", message, receiverId, senderId, conversationId);
    $("#messageInput").val("");
});

// Unfortunately, Sanitizer does not work here :(
function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function updateScroll() {
    $([document.documentElement, document.body]).animate({
        scrollTop: $("#messageInput").offset().top
    }, 10);
}

function formatDateTime() {
    let localTime = new Date();
    let time = moment.utc(localTime).local();
    let formattedDateTime = time.format("llll");
    return formattedDateTime;
}