let lengthInput = document.querySelector(".length");
let arrowUp = document.querySelector(".arrowUp");
let arrowDown = document.querySelector(".arrowDown");

lengthInput.value = 16;

async function copyToClipboard(textToCopy) {
    // Navigator clipboard api needs a secure context (https)
    if (navigator.clipboard && window.isSecureContext) {
        await navigator.clipboard.writeText(textToCopy);
    } else {
        // Use the 'out of viewport hidden text area' trick
        const textArea = document.createElement("textarea");
        textArea.value = textToCopy;
            
        // Move textarea out of the viewport so it's not visible
        textArea.style.position = "absolute";
        textArea.style.left = "-999999px";
            
        document.body.prepend(textArea);
        textArea.select();

        try {
            document.execCommand('copy');
        } catch (error) {
            console.error(error);
        } finally {
            textArea.remove();
        }
    }
}

function showError(message) {
    let notification = document.querySelector(".notification");
    notification.classList.add("red");
    notification.innerHTML = message;
    setTimeout(() => {}, 300)

    notification.classList.add("shown");
    setTimeout(() => {
        notification.classList.add("notshown");
        notification.classList.remove("shown");
    }, 4000)
}

function showCopiedNotification() {
    let notification = document.querySelector(".notification");
    notification.classList.remove("red");
    notification.innerHTML = "Copied!";

    notification.classList.add("shown");
    setTimeout(() => {
        notification.classList.add("notshown");
        notification.classList.remove("shown");
    }, 500)
}

function copyText(element) {
    let copyText = document.querySelector(element).innerHTML;
    
    copyToClipboard(copyText);
    showCopiedNotification();
}

arrowUp.addEventListener("click", () => {
    currentValue = parseInt(lengthInput.value);
    if (isNaN(currentValue)){ 
        currentValue = 1;
    }
    currentValue += 1;
    if (currentValue > 256) {
        currentValue = 256;
    }
    lengthInput.value = currentValue;
});

arrowDown.addEventListener("click", () => {
    currentValue = parseInt(lengthInput.value);
    if (isNaN(currentValue)){ 
        currentValue = 1;
    }
    currentValue = parseInt(lengthInput.value);
    currentValue -= 1;
    if (currentValue < 1) {
        currentValue = 1;
    }
    lengthInput.value = currentValue;
});

document.querySelector(".length").addEventListener("keypress", function (evt) {
    if (evt.which != 8 && evt.which != 0 && evt.which < 48 || evt.which > 57)
    {
        evt.preventDefault();
    }
});

document.querySelector("input.length").addEventListener("input", function () {
    let input = document.querySelector(".length");
    input.addEventListener('input', function() {
        const enteredValue = parseInt(input.value);
        if (enteredValue > 256) {
            input.value = 256; // set value to maximum allowed
        } 
        else if (enteredValue < 1) {
            input.value = 1;
        }
    });
})

function displayData(data) {
    document.querySelector(".generatedPassword").innerHTML = data.password;
    document.querySelector(".generatedPoem").innerHTML = data.poem;
}

function generate() {
    let length = document.querySelector(".length").value;
    let reverseMode = document.querySelector(".reverseModeInput").checked;
    let includeNumbers = document.querySelector(".includeNumbersInput").checked;
    let removeAllSpecialSymbols = document.querySelector(".removeSpecialSymbolsInput").checked;
    let capitalizeNouns = document.querySelector(".capitalizeNouns").checked;
    let capitalizeVerbs = document.querySelector(".capitalizeVerbs").checked;
    let capitalizePronouns = document.querySelector(".capitalizePronouns").checked;
    let separator = document.querySelector('input[name="radio"]:checked').value;

    let data = JSON.stringify({
        "length": length,
        "reverseMode": reverseMode,
        "includeNumbers": includeNumbers,
        "removeAllSpecialSymbols": removeAllSpecialSymbols,
        "separator": separator,
        "capitalizeNouns": capitalizeNouns,
        "capitalizeVerbs": capitalizeVerbs,
        "capitalizePronouns": capitalizePronouns
    })

    $.ajax({
        type: "POST",
        url: "/Generate",
        data: data,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.error.length > 1) 
            {
                showError(result.error);
            }
            else 
            {
                displayData(result);
                document.querySelector(".passwordResultBlock").classList.add("visible");
                document.querySelector(".poemResultBlock").classList.add("visible");
            }
        },
        dataType: "json"
    });
}

document.querySelector(".generate").addEventListener("click", function () {
    generate();
})