var mongoose = require('mongoose');

// mongoose.connect('mongodb://127.0.0.1:27017/Pic2Word_API')
//  .then(() => console.log('cate!'));

var puzzle = new mongoose.Schema({
    cate_name:{
        type:String
    },
    image:{
        type:String
    }
});
module.exports = mongoose.model('Category',puzzle);
