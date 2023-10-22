var express = require("express");
var router = express.Router();
let controller = require("../controllers/tipoController");

/* GET tipos. */
router.get("/getTipos", function (req, res, next) {
  controller.tipos_get(req, res, next);
});

module.exports = router;
