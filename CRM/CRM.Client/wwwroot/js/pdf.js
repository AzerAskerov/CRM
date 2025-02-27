function generatePDF() {
    document.documentElement.scrollTop = 0;
    // Choose the element that your content will be rendered to.
    const element = document.getElementById('print');
    const options = {
        image: { type: 'png', quality: 1 },
        html2canvas: { scale: 3 },
        margin: [10, 0, 10, 0],
        pagebreak: { mode: ["avoid-all", "css"], before: '#pageX' }
    }
    // Choose the element and save the PDF for your user.
    html2pdf().from(element).set(options).toPdf().get("pdf").save();
}