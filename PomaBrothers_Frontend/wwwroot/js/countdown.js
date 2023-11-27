var countdownValue = 900;
var countdownElement = document.getElementById("countdown");
var countdownValueElement = document.getElementById("countdown-value");

function startCountdown() {
    countdownElement.style.display = "block";
    var intervalId = setInterval(function () {
        countdownValue--;
        countdownValueElement.innerText = countdownValue;
        if (countdownValue <= 0) {
            clearInterval(intervalId);
            showAlertAndLogout();
        }
    }, 1000);
}

function logout() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Login/Logout", true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            window.location.href = "/Login/Index";
        }
    };
    xhr.send();
}

function showAlertAndLogout() {
    // Muestra una alerta personalizada
    alert('Su sesión a caducado, vuelva iniciar sesion');

    // Llama al método de logout después de que el usuario hace clic en "Aceptar"
    logout();
}

window.addEventListener("mousemove", function () {
    countdownValue = 900;
    countdownElement.style.display = "none";
});

startCountdown();

