function extractAndPrintContent() {
    // Get the content of the div to be printed
    var contentToPrint = document.getElementById('Printcontent').innerHTML;

    // Open the second page in a new window
    var printWindow = window.open('/Admin/Home/Print', '_blank');

    // Once the second page is loaded, transfer the content and trigger printing
    printWindow.onload = function () {
        // Insert the transferred content into the second page
        printWindow.document.getElementById('transferredContent').innerHTML = contentToPrint;

        // Print the second page
       // printWindow.print();

        // Print the second page
        var isPrinted = printWindow.print();

        // Close the print window if print dialog was successfully displayed
        if (isPrinted) {
            printWindow.close();
        }
    };
}