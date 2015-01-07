Links: 
1. http://engineering.linkedin.com/incubator/creating-product-tours-hopscotch
2. https://github.com/linkedin/hopscotch
3. http://linkedin.github.io/hopscotch/

Sample code:
var tour = {
            id: "MyID",
            steps: [
                {
                    target: 'aFeedback',
                    title: 'Submit Feedback',
                    content: 'Discuss any feedback that you may have here',
                    placement: 'bottom'
                },
                {
                    target: 'chart-container',
                    placement: 'left',
                    title: 'Timeline visualization Charts',
                    content: '<content here>'
                },
                {
                    target: 'btnReset',
                    placement: 'bottom',
                    title: 'Reset Button',
                    content: 'It clears any filters .'
                }
              ],
            showPrevButton: true,
            scrollTopMargin: 100
        };
        
        //Angular scope object "ng-click=TakeTour()" in html file. Thats all
        $scope.TakeTour = function () {
            hopscotch.startTour(tour);
        };
