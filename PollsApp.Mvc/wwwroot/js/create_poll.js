
var hasEndDateCkbx = document.getElementById("ckbx-has-end-date");
var hasEndDate = hasEndDateCkbx.checked;
var datepicker = document.getElementById("datepicker");

try {
    $(function () {
        $("#datepicker").datepicker();
    });
}
catch (e) { }

console.log(hasEndDate);

/*if (hasEndDateCkbx.checked) {
    datepicker.className = "datepicker-visible";
}*/

document.getElementById("add-option").addEventListener("click", () => {
    let optionItem = document.createElement("div");
    optionItem.className = "option-item-create";
    
    let option = document.createElement("input");
    option.className = "poll-option-input";
    option.setAttribute('type', 'text');
    option.setAttribute('name', 'options');

    optionItem.appendChild(option);

    let removeButton = document.createElement("div");
    removeButton.className = "remove-option";
    optionItem.appendChild(removeButton);
    removeButton.innerHTML = "&#10060;";
    removeButton.addEventListener("click", (e) => {
        console.log(e);
        return e.currentTarget.parentNode.remove();
    });

    

    document.getElementById("options-container").appendChild(optionItem);

});

hasEndDateCkbx.addEventListener("change", () => {
    setTimeout(() => {
        console.log("ckbx");
        console.log(hasEndDate);
        hasEndDate = !hasEndDate;
        hasEndDateCkbx.value = hasEndDate;
        console.log(hasEndDate);
        if (hasEndDate) {
            datepicker.className = "datepicker-visible";
        }
        else {
            datepicker.className = "datepicker-hidden";
        }
    }, 100);
    
});