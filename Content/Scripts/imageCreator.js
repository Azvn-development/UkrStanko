// Триггер на открытие окна загрузки файла
function downloadImage(imageContainer, imageInput, loadedImage) {
    // Обработчик удаления изображения
    deleteImageContainerHandler(loadedImage);

    imageContainer.click(function (event) {
        if (!$("[name='deleteImageButton']").is(event.target)) {
            //Вызов события клик по окну загрузки
            imageInput.trigger("click");

            //Обработка события выбора файлов
            imageInput.change(function (event) {
                //Выбранные файлы
                let files = event.target.files;

                if (files.length > 0) {
                    // Ресайз выбранного файла
                    resizeImage(files[0], imageContainer);

                    // Обнуление поля загруженного на сервер файла
                    if (loadedImage != null) loadedImage.val("");

                    // Обнуление файлов в input
                    event.target.value = "";
                } //if
            });
        } // if
    })
} // downloadUserFile

// Ресайз файла
function resizeImage(file, imageContainer) {
    // Максимальные размеры изображения
    let imgMaxWidth = 1000;
    let imgMaxHeight = 450;

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

        let bytes = canvas.toDataURL('image/jpeg', 0.9);

        // Создание контейнера для изображения и добавление в список
        createImageContainer(imageContainer, img, bytes);
    }

    // Добавление в изображение файла
    img.src = URL.createObjectURL(file);
} // resizeImage

// Создание контейнера с измененным изображением
function createImageContainer(imageContainer, img, bytes) {
    // Добавление фона загруженной картинки
    imageContainer.css("background", `url(${img.src}) 50% 50% no-repeat`);
    imageContainer.css("background-size", "cover");
    imageContainer.addClass("active");

    // Создание textarea
    let textarea = document.getElementById("Image");
    if (textarea != null) {
        $(textarea).val(bytes);
    } else {
        textarea = document.createElement("textarea");
        $(textarea).attr("id", "Image");
        $(textarea).attr("name", "Image");
        $(textarea).attr("hidden", true);
        $(textarea).val(bytes);
    
        // Добавление textarea к контейнеру
        imageContainer.append(textarea);
    } // if
} // createImageContainer

// Удаление загруженного изображения пользователя
function deleteImageContainerHandler(loadedImage) {
    $("body").delegate("[name='deleteImageButton']", "click", function () {
        // Удаление пути загруженного файла
        if (loadedImage != null) loadedImage.val("");

        // Удаление внутренних стилей из блока картинки
        $(this).parents("div").removeClass("active");
        $(this).parents("div").removeAttr("style");
    });
} // deleteImageContainerHandler