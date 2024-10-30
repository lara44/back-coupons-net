import { ref } from "vue";
import axios from "axios";
import * as yup from "yup";
import QRCode from "qrcode";
import { saveAs } from "file-saver";

const validationSchema = yup.object().shape({
  identification: yup
    .string()
    .required("La identificación es obligatoria")
    .matches(/^[0-9]+$/, "La identificación solo debe contener números")
    .min(10, "La identificación debe tener al menos 10 caracteres"),
});

export const useCouponsByClient = () => {
  const identification = ref("");
  const coupons = ref([]);
  const errors = ref({});

  const validateIdentification = async () => {
    try {
      await validationSchema.validate({ identification: identification.value });
      errors.value.identification = "";
      return true;
    } catch (validationError) {
      errors.value.identification = validationError.message;
      return false;
    }
  };

  const searchCoupons = async () => {
    const isValid = await validateIdentification();
    if (!isValid) return false;

    try {
      const { data } = await axios.get(
        `/api/redeem/GetCouponsByClientIdentification/${identification.value}`
      );
      console.log(data);
      coupons.value = data.data;
      return false;
    } catch (error) {
      console.error("Error al buscar los cupones:", error);
      coupons.value = [];
      return true;
    }
  };

  const QrCoupon = async (coupon) => {
    try {
      const qrCodeDataUrl = await QRCode.toDataURL(coupon.url);
      const blob = await (await fetch(qrCodeDataUrl)).blob();
      saveAs(blob, `${coupon.coupon.couponCode}.png`);
    } catch (error) {
      console.error("Error generating QR code", error);
    }
  };

  return {
    identification,
    coupons,
    errors,
    searchCoupons,
    QrCoupon,
  };
};
