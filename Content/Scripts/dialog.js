// Показ диалогового окна
function showDialog(dialog) {
    $("body").css("overflow", "hidden");
    $(dialog).css("display", "flex");
} // showDialog

// Скрытие диалогового окна
function hideDialog() {
    $(".dialog-modal").css("display", "none");
    $("body").css("overflow", "auto");
} // hideDialog

//Очиста диалогового окна сообщений
function clearMessageDialog(field, response) {
    $(field).val("");

    $(response).attr("hidden", true);
    $(response).children("div")
        .each((i, item) => $(item).html(""));
    $(response).children("input")
        .each((i, item) => $(item).val(""));
} // clearMessageDialog

//Очиста диалогового окна фильтров
function clearFilterDialog(fields, checkBoxes) {
    $(fields).each((i, item) => $(item).val(""));
    $(checkBoxes).each((i, item) => $(item).prop("checked", false));
} // clearFilterDialog
