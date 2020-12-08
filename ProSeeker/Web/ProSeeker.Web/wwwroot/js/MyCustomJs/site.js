function getRatings() {
    let ratings = document.querySelectorAll('.number-rating');
    let starsRef = document.querySelectorAll('.stars-inner');
    const starsTotal = 5;

    for (i = 0; i < ratings.length; i++) {
        let rating = ratings[i].textContent;
        // Get percentage
        const starPercentage = (rating / starsTotal) * 100;
        // Round to nearest 10
        const starPercentageRounded = `${Math.round(starPercentage / 10) * 10}%`;
        // Set width of stars-inner to percentage            
        starsRef[i].style.width = starPercentageRounded;
    }
}