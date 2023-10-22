var express = require("express");
var router = express.Router();
let controller = require("../controllers/maquinaController");

/* GET maquinas. */
router.get("/getMaquinas", function (req, res, next) {
  controller.maquinas_getAll(req, res, next);
});

/* GET maquina. */
router.get("/getMaquina/:id", function (req, res, next) {
  console.log("Entre al route");
  controller.maquina_get(req, res, next);
});

/* GET maquina. */
router.get("/getMaquinasAndType", function (req, res, next) {
  controller.maquinasAndType(req, res, next);
});

module.exports = router;
