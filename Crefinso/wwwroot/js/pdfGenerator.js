// pdfGenerator.js
window.generatePdf = function (elementId, fileName) {
    // Verifica si jsPDF y html2canvas están cargados
    if (typeof jspdf !== 'undefined' && typeof html2canvas !== 'undefined') {
        const element = document.getElementById(elementId);
        if (!element) {
            console.error('Elemento no encontrado:', elementId);
            return;
        }

        // Crea una instancia de jsPDF
        const pdf = new jspdf.jsPDF('p', 'pt', 'a4');

        // Define los márgenes
        const margin = 20; // Margen de 20 puntos en todos los lados

        // Convierte el contenido del elemento a PDF usando html2canvas
        html2canvas(element, { scale: 2 }).then((canvas) => {
            const imgData = canvas.toDataURL('image/png');
            const imgWidth = pdf.internal.pageSize.getWidth() - 2 * margin; // Ancho de la imagen con márgenes
            const imgHeight = (canvas.height * imgWidth) / canvas.width; // Alto de la imagen proporcional

            // Agrega la imagen al PDF con márgenes
            pdf.addImage(imgData, 'PNG', margin, margin, imgWidth, imgHeight);
            // Guarda el PDF con el nombre proporcionado
            pdf.save(fileName);
        });
    } else {
        console.error('jsPDF o html2canvas no están cargados.');
    }
};