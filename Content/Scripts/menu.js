// Скрытие всех выпадающих меню
function hideDropDownMenu(event) {
    let eventTargetPatent = $(event.target).parent(".header-dropdown-toggle");

    if (!$(".header-dropdown-toggle").is(event.target) && !$(eventTargetPatent).hasClass("header-dropdown-toggle")) {
        $(".header-dropdown-toggle").removeClass("open");
        $(".header-dropdown-list").removeClass("open");
    } //if
} // showDropDownMainMenu

// Показ главного выпадающего меню
function showDropDownMenu(menuId, menuToggle) {
    // Скрытие других выпадающих меню
    $(".header-dropdown-toggle").removeClass("open");
    $(".header-dropdown-list").removeClass("open");

    // Открытие текущего меню
    $(menuId).addClass("open");
    $(menuToggle).addClass("open");
} // showDropDownMainMenu