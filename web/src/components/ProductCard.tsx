import Image from "next/image";

interface Props {
 title: string;
 image: {
  src: string;
  alt: string;
  width: number;
  height: number;
 };
 price: string;
}

export const ProductCard = ({ title, image, price }: Props) => {
 return (
  <div className="block group">
   <Image
    src="https://images.unsplash.com/photo-1592921870789-04563d55041c?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=774&q=80"
    alt={image.alt}
    className="object-cover rounded aspect-square"
    width={image.width}
    height={image.height}
   />

   <div className="mt-3 flex justify-between">
    <div>
     <p className="font-medium text-gray-900 group-hover:underline group-hover:underline-offset-4">{title}</p>

     <p className="mt-1 text-sm text-gray-700">{price} PLN</p>
    </div>
    <div>
     <button>🌴</button>
    </div>
   </div>
  </div>
 );
};
