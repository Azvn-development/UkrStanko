// Валидация введенного значения в поле
function checkField(ui, field) {
    if (ui.item == null) {
        $(field).val("");
    } // if
} // checkField

// Автозаполнение аналогов оборудования
function analogsAutocomplete(field, machineTypeIdField, machines, currentMachineParentTypeName, currentMachineId) {
    if (machines != null) {
        $(field).autocomplete({
            appendTo: $(field).parent(),
            source: machines
                .filter(i => i["id"] != currentMachineId && i["machineType"]["parentMachineType"]["name"] == currentMachineParentTypeName)
                .map(i => i["name"]),
            change: function (event, ui) {
                checkField(ui, field)

                if (ui.item != null) {
                    let machine = machines.filter(i => i["name"] == ui.item.value);
                    $(machineTypeIdField).val(machine[0]["machineTypeId"]);
                }
            }
        })
    }
} // analogsAutocomplete

// Автозаполнение поля типов оборудования
function machineTypesAutocomplete(field, analogueField, analogueMachineTypeIdField, machines, machineTypes, currentMachineId) {
    if (machineTypes != null) {
        $(field).autocomplete({
            appendTo: $(field).parent(),
            source: machineTypes.map(i => i["name"]),
            change: function (event, ui) {
                checkField(ui, field)

                $(analogueField).val("");
                $(analogueMachineTypeIdField).val(0);

                if (ui.item != null) {
                    analogsAutocomplete(analogueField,
                        analogueMachineTypeIdField,
                        machines,
                        ui.item.value,
                        currentMachineId);
                } // if
            }
        })
    } // if
} // machineTypesAutocomplete

// Автозаполнение поля ролей пользователя
function userRolesAutocomplete(field, roles) {
    if (roles != null) {
        $(field).autocomplete({
            appendTo: $(field).parent(),
            source: roles.map(i => i["name"]),
            change: function (event, ui) {
                checkField(ui, field)
            }
        })
    } // if
} // userRolesAutocomplete