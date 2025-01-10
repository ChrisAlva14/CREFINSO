function printSection(elementId) {
    // Obtén el elemento que deseas imprimir
    const printElement = document.getElementById(elementId);

    // Oculta todos los elementos del body
    const elements = document.body.children;
    for (let i = 0; i < elements.length; i++) {
        elements[i].style.visibility = 'hidden';
    }

    // Muestra solo el elemento que deseas imprimir
    if (printElement) {
        printElement.style.visibility = 'visible';
    }

    // Llama a la función de impresión del navegador
    window.print();

    // Restaura la visibilidad de todos los elementos
    for (let i = 0; i < elements.length; i++) {
        elements[i].style.visibility = 'visible';
    }
}

function openPdfInNewTab(pdfUrl) {
    window.open(pdfUrl, "_blank");
}