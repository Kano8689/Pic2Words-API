var mongoose = require('mongoose');

// mongoose.connect('mongodb://127.0.0.1:27017/Pic2Word_API')
//  .then(() => console.log('pzl!'));

var puzzle = new mongoose.Schema({
    pzl_name:{
        type:String
    },
    image:{
        type:String
    },
    pzl_word:{
        type:String
    },
    cate_id:{
        type:String
    }
    // status:{
    //     type:Int16Array
    // }
});
module.exports = mongoose.model('Puzzle',puzzle);
