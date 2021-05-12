// Create an instance of the Stripe object with your publishable API key
var stripe = Stripe("pk_test_51IqCcAH8Hw1wc8VbCb4JHYUGubqI7GN4T5aqHzVavBo8mQoSQazVaAOwUBfEXwz7wXA6hwoeAMkgaQzulzIWrtul00rWo4RXWe");

// butonul este generat dinamic, motiv pentru care se foloseste aceasta abordare pt evenimentul click
$(document).on('click', "#checkout-button", function () {
    fetch("/Clienti/Informatii/Charge", {
        method: "POST",
    })
        .then(function (response) {
            alert("Response", response);
            return response.json();
        })
        .then(function (session) {
            alert("Session", session);
            // se redirectioneaza inspre checkout-ul oferit de stripe
            return stripe.redirectToCheckout({ sessionId: session.id });
        })
        .then(function (result) {
            alert(result);
            console.log(result);
            // If redirectToCheckout fails due to a browser or network
            // error, you should display the localized error message to your
            // customer using error.message.
            if (result.error) {
                alert(result.error.message);
            }
        })
        .catch(function (error) {
            console.error("Error:", error);
        });
})