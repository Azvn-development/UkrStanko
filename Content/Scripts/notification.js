// Вывод пуш уведомления из хаба
function setHubConnection() {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    // Получение уведомления об ошибке от сервера
    hubConnection.on("Error", function (message) {
        let messageElement = $("#pop-up-message");

        messageElement.removeClass("success");
        messageElement.addClass("error");
        messageElement.children("i").attr("class", "fa fa-2x fa-exclamation-triangle")
        messageElement.children("p").html(message);
        messageElement.show();
        setTimeout(function () { messageElement.fadeOut("slow"); }, 3000);
    });

    // Получение уведомления об успешной операции от сервера
    hubConnection.on("Success", function (message) {
        let messageElement = $("#pop-up-message");

        messageElement.removeClass("error");
        messageElement.addClass("success");
        messageElement.children("i").attr("class", "fa fa-2x fa-check-circle")
        messageElement.children("p").html(message);
        messageElement.show();
        setTimeout(function () { messageElement.fadeOut("slow"); }, 3000);
    });

    hubConnection.start().then(() => sessionStorage.setItem("connectionId", hubConnection.connectionId));
} // setHubConnection