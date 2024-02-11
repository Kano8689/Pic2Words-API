var add_category = require('../model/Add_Category');
var add_puzzle = require('../model/Add_Puzzle');

exports.Add_Category = async (req,res) => {
    var img_name = req.file.originalname;
    req.body.image = img_name;
    var data = await add_category.create(req.body);
    // res.status(200).json({
    //     status:"Success Add Category",
    //     data
    // })
    res.status(200).json(data)
};

exports.View_Category = async (req,res) => {
    var data = await add_category.find();
    // res.status(200).json({
    //     status:"Success View Category",
    //     data
    // })
    res.status(200).json(data)
};

exports.Add_Puzzle = async (req,res) => {
    var img_name = req.file.originalname;
    req.body.image = img_name;
    var data = await add_puzzle.create(req.body);
    // res.status(200).json({
    //     status:"Success Add Puzzle",
    //     data
    // })
    res.status(200).json(data)
};

exports.Get_Puzzle = async (req,res) => {
    var cate_id = req.params.cate_id;
    var data = await add_puzzle.find({"cate_id":cate_id});
    // res.status(200).json({
    //     status:"Success Get Puzzle By Id",
    //     data
    // })
    res.status(200).json(data)
};

exports.Single_Puzzle = async (req,res) => {
    var pzl_id = req.params.pzl_id;
    var data = await add_puzzle.findById(pzl_id);
    // res.status(200).json({
    //     status:"Success Get Puzzle By Id",
    //     data
    // })
    res.status(200).json([data])
};
