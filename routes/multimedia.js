var express = require("express");
var router = express.Router();
let controller = require("../controllers/multimediaController");

router.get("/getMultimedia/:id", (req, res, next) => {
  controller.getMultimedia(req, res, next);
});

module.exports = router;
