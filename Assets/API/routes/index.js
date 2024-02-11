var express = require('express');
var router = express.Router();
const multer = require('multer');

var storage = multer.diskStorage({
    destination: function (req, file, cb) {
      cb(null, 'public/images')
    },
    filename: function (req, file, cb) {
      cb(null, file.originalname)
    }
  })
  var upload = multer({ storage: storage });

var {Add_Category,View_Category,Add_Puzzle,Get_Puzzle,Single_Puzzle} = require('../controller/usercontroller');

router.post('/Add_Category',upload.single('image'),Add_Category);
router.get('/View_Category',View_Category);
router.post('/Add_Puzzle',upload.single('image'),Add_Puzzle);
router.get('/Get_Puzzle/:cate_id',Get_Puzzle);
router.get('/Single_Puzzle/:pzl_id',Single_Puzzle);

module.exports = router;
