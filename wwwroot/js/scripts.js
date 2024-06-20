// scripts.js

$(document).ready(function () {
    // Star rating hover effect
    $('.star-rating .star').hover(function () {
        var currentRating = $(this).data('value');
        $(this).siblings('.star').css('color', 'gray'); // Reset all stars to gray
        $(this).prevAll('.star').addBack().css('color', 'gold'); // Highlight stars up to the current one
    }, function () {
        // Mouse leave event (reset to the current rating)
        var rating = $(this).closest('.star-rating').data('current-rating');
        $(this).siblings('.star').css('color', 'gray');
        $(this).prevAll('.star').addBack().slice(0, rating).css('color', 'gold');
    });

    // Click event to set the rating
    $('.star-rating .star').click(function () {
        var newRating = $(this).data('value');
        $(this).siblings('.star').css('color', 'gray');
        $(this).prevAll('.star').addBack().css('color', 'gold');
        $(this).closest('.star-rating').data('current-rating', newRating); // Store the current rating
        var movieId = $(this).closest('.star-rating').data('movie-id');
        // You can optionally send an AJAX request to save the rating
        // Example:
        // $.post('/Movies/RateMovie', { movieId: movieId, rating: newRating }, function(data) {
        //    console.log('Rating saved successfully.');
        // });
    });
});
