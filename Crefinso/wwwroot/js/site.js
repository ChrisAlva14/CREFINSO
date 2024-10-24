function mostrarAlertaCargando() {
    let timerInterval;
    Swal.fire({
        title: "Cargando datos...",
        html: "Esta alerta se cerrará en <b></b> milisegundos.",
        timer: 1000,
        timerProgressBar: true,
        didOpen: () => {
            Swal.showLoading();
            const timer = Swal.getPopup().querySelector("b");
            timerInterval = setInterval(() => {
                timer.textContent = `${Swal.getTimerLeft()}`;
            }, 100);
        },
        willClose: () => {
            clearInterval(timerInterval);
        }
    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.timer) {
            console.log("Alerta cerrada automáticamente por el temporizador.");
        }
    });
}
