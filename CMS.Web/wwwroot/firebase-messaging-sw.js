importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-messaging-compat.js');

var config = {
    apiKey: "AIzaSyCRBkweLq-HAx8PRUAB2w35-SrRRBegCXI",
    authDomain: "cmsweb-4ecf6.firebaseapp.com",
    projectId: "cmsweb-4ecf6",
    storageBucket: "cmsweb-4ecf6.firebasestorage.app",
    messagingSenderId: "48754161261",
    appId: "1:48754161261:web:4f5193b93fc74d1bdff164",
    measurementId: "G-LSBCBTCP67"
};

firebase.initializeApp(config);

const messaging = firebase.messaging();
//the method was changed bc it is old
messaging.onBackgroundMessage(function (payload) {
    //// Customize notification here
    var notificationTitle = 'My Title';
    var notificationOptions = {
        body: payload.data.body
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});