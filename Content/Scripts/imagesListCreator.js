// Триггер на открытие окна загрузки файлов
function downloadImages(openDialogButton, imagesInput, imagesList) {
    // Обработчик удаления изображения
    deleteImageLiHandler();

    // Обработчик открытия диалогового окна
    openDialogButton.click(function () {
        //Вызов окна загрузки файлов
        imagesInput.trigger("click");

        //Обработка события выбора файлов
        imagesInput.change(function (event) {
            //Выбранные файлы
            let files = event.target.files;

            if (files.length > 0) {
                // Формирование списка из выбранных файлов
                for (let i = 0; i < files.length; i++) {
                    // Ресайз выбранного файла
                    resizeListImage(files[i], imagesList);
                } // for
            } //if

            event.target.value = '';
        });
    })
} // downloadImages

// Ресайз файла
function resizeListImage(file, imagesList) {
    // Максимальные размеры изображения
    let imgMaxWidth = 1000;
    let imgMaxHeight = 600;

    // Создание контейнера для ресайза файла
    let canvas = document.createElement('canvas');
    let ctx = canvas.getContext("2d");

    // Создание изображения для контейнера
    let img = document.createElement('img');
    img.onload = function () {
        // Размеры файла
        let imgWidth = img.width;
        let imgHeight = img.height;

        // Пропорции файла
        let scale = img.width / img.height;

        // Ресайз файла с сохранением пропорций
        let newImgWidth = imgWidth;
        let newImgHeight = imgHeight;
        if (img.width > img.height) {
            newImgWidth = imgMaxWidth;
            newImgHeight = imgMaxWidth / scale;
        } else {
            newImgWidth = imgMaxHeight * scale;
            newImgHeight = imgMaxHeight;
        } // if

        // Запись измененного файла в контейнер
        canvas.width = newImgWidth;
        canvas.height = newImgHeight;
        ctx.drawImage(img, 0, 0, newImgWidth, newImgHeight);

        let bytes = canvas.toDataURL('image/jpeg', 0.95);

        // Создание контейнера для изображения и добавление в список
        imagesList.append(createImageLi(img, bytes));
    }

    // Добавление в изображение файла
    img.src = URL.createObjectURL(file);
} // resizeListImage

// Создание контейнера с измененным изображением
function createImageLi(img, bytes) {
    // Создание тега li для списка
    let li = document.createElement("li");

    // Создание тега i
    let i = document.createElement("i");
    $(i).attr("name", "deleteImageButton");
    $(i).addClass("fa fa-minus-square fa-2x icon-cancel"); // добавление классов

    // Создание textarea
    let textarea = document.createElement("textarea");
    $(textarea).attr("id", "Images");
    $(textarea).attr("name", "Images");
    $(textarea).attr("hidden", true);
    $(textarea).val(bytes);

    // Добавление тегов к пункту списка
    li.append(img);
    li.append(i);
    li.append(textarea);

    return li;
} // createImageLi

// Удаление контейнера с измененным изображением
function deleteImageLiHandler() {
    $("body").delegate("[name='deleteImageButton']", "click", function () {
        $(this).parents("li").remove();
    });
} // deleteImageLiHandler
