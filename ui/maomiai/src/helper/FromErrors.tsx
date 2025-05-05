//Array.isArray(messages) ? messages : [String(messages)]
export default async function Parse400Error(errors: Record<string, string[]>){
  if (Object.keys(errors).length > 0) {
    let fields = Object.entries(errors).map(([_, errorMessages]) => {
      let messages = Object.values(errorMessages)[0];

      return {
        name: Object.keys(errorMessages)[0],
        errors: Array.isArray(messages) ? messages : [String(messages)],
      };
    });

    return fields;
  }
}
