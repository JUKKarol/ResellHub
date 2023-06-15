import { ProductCard } from "@/components/ProductCard";
import Link from "next/link";

export interface Offer {
 title: string;
 brand: string;
 category: string;
 description: string;
 condition: string;
 pricePLN: string;
 productionYear: string;
 userId: string;
 encodedName: string;
}

const getAllOffers = async () => {
 const data: Offer[] = await fetch("http://localhost:8080/api/Offer", { cache: "no-cache" }).then((res) => res.json());
 return data;
};

export default async function Home() {
 const data = await getAllOffers();
 return (
  <main className="flex flex-col gap-4 p-8 md:p-20">
   <h1 className="text-4xl font-bold">Najnowsze oferty</h1>
   <div className="grid grid-cols-2 md:grid-cols-4 gap-2 md:gap-4">
    {data.map(({ title, pricePLN }) => (
     <Link key={title} href={`/offers/${title}`}>
      <ProductCard title={title} price={pricePLN} image={{ src: "", alt: "", height: 300, width: 300 }} />
     </Link>
    ))}
   </div>
  </main>
 );
}
