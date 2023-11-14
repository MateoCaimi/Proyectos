var express = require("express");
var router = express.Router();
var multer = require("multer");
const path = require("path");
let controller = require("../controllers/multimediaController");

// Configura la ubicación y el nombre del archivo en el servidor
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, "public/images"); // Carpeta donde se guardarán las imágenes
  },
  filename: (req, file, cb) => {
    cb(
      null,
      file.fieldname + "-" + Date.now() + path.extname(file.originalname)
    );
  },
});

const upload = multer({ storage: storage });

router.post("/upload", upload.single("image"), function (req, res, next) {
  console.log("Entre al controller upload");
  controller.upload(req, res, next);
});

module.exports = router;
