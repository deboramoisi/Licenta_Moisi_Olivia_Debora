// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
