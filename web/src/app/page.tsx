interface Offer {
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

export default async function Home() {
 const data: Offer[] = await fetch("http://localhost:8080/api/Offer/offers").then((res) => res.json());
 return (
  <main className="flex flex-col gap-4 p-20">
   <h1 className="text-lg">Oferty</h1>
   {data.map((offer) => (
    <div className="border" key={offer.title}>
     <h2>{offer.title}</h2>
     <p>{offer.description}</p>
    </div>
   ))}
  </main>
 );
}
