// Initialize the owl carousel using JQuery

$(document).ready(function () {

	// banner owl carousel
	$("#banner-area .owl-carousel").owlCarousel({
		// Specify parameters
		loop: true,
		dots: true,
		items: 1
	});

});

function getExtensie(data) {
    var icon = "";
    if (data.includes("pdf")) {
        icon = "far fa-file-pdf";
    } else if (data.includes("docx") || data.includes("doc")) {
        icon = "far fa-file-word";
    } else if (data.includes("xlsx") || data.includes("csv")) {
        icon = "far fa-file-excel";
    } else if (data.includes("png") || data.includes("jpg") || data.includes("jpeg") || data.includes("img")) {
        icon = "far fa-images";
    }
    return icon;
}